using Database;
using Database.Entity;
using Database.Entity.Id;
using Microsoft.EntityFrameworkCore;
using Presentation.Service;
using Presentation.Service.Exception;

namespace Application.Service;

public class NavService(
    TimeProvider timeProvider,
    ApplicationContext applicationContext) : INavService
{
    public async Task<List<NavNodeEntity>> AddNavNode(
        string map,
        string tag,
        int x,
        int y,
        int z,
        List<NavNodeEntityId> links)
    {
        var distinctList = links.Distinct().ToList();
        var foundLinkNodes = await applicationContext.NavNode
            .Where(node =>
                node.Map == map &&
                node.Tag == tag &&
                distinctList.Contains(node.Id))
            .Include(node => node.LinkedTo)
            .Include(node => node.LinkedFrom)
            .AsSingleQuery()
            .ToListAsync();

        if (foundLinkNodes.Count != distinctList.Count)
        {
            throw new GmodException("Did not find all the links when creating node.");
        }
        
        var newNode = new NavNodeEntity
        {
            Id = new NavNodeEntityId(Guid.CreateVersion7()),
            LinkedTo = foundLinkNodes,
            LinkedFrom = foundLinkNodes,
            Tag = tag,
            Map = map,
            X = x,
            Y = y,
            Z = z,
            CreatedAt = timeProvider.GetLocalNow().DateTime.ToUniversalTime(),
        };
        
        applicationContext.NavNode.Add(newNode);
        foreach (var node in foundLinkNodes)
        {
            node.LinkedTo.Add(newNode);
            node.LinkedFrom.Add(newNode);
        }
        
        await applicationContext.SaveChangesAsync();

        return [newNode, ..foundLinkNodes];
    }

    public async Task<List<NavNodeEntity>> RemoveNavNode(
        string map,
        string tag,
        NavNodeEntityId id)
    {
        var foundNode = await applicationContext.NavNode
            .Include(node => node.LinkedTo)
            .Include(node => node.LinkedFrom)
            .AsSingleQuery()
            .FirstOrDefaultAsync(node
                => node.Map == map && 
                   node.Tag == tag &&
                   node.Id == id);

        if (foundNode is null)
        {
            throw new GmodException("Did not find node");
        }

        applicationContext.NavNode.Remove(foundNode);
        await applicationContext.SaveChangesAsync();
        return [..foundNode.LinkedTo, ..foundNode.LinkedFrom];
    }

    public async Task<List<NavNodeEntity>> AppendNavNodeLink(
        string map,
        string tag,
        NavNodeEntityId nodeA,
        NavNodeEntityId nodeB)
    {
        var nodeIdPair = new List<NavNodeEntityId>([nodeA, nodeB]);
        var nodePair = await applicationContext.NavNode
            .Include(node => node.LinkedTo)
            .Include(node => node.LinkedFrom)
            .Where(node
                => node.Map == map && 
                   node.Tag == tag &&
                   nodeIdPair.Contains(node.Id))
            .AsSingleQuery()
            .ToListAsync();

        if (nodePair.Count != 2)
        {
            throw new GmodException($"Found {nodePair.Count} node instead of 2 when appending a link.");
        }

        if (nodePair[0].Id == nodePair[1].Id)
        {
            throw new GmodException("Duplicate node found (this should never happen).");
        }

        if (nodePair[0].LinkedTo.Contains(nodePair[1]))
        {
            throw new GmodException($"node <{nodePair[0].Id}> is already linked to node <{nodePair[1].Id}>.");
        }
        
        if (nodePair[1].LinkedTo.Contains(nodePair[0]))
        {
            throw new GmodException($"node <{nodePair[1].Id}> is already linked to node <{nodePair[0].Id}>.");
        }
        
        nodePair[0].LinkedTo.Add(nodePair[1]);
        nodePair[0].LinkedFrom.Add(nodePair[1]);
        
        nodePair[1].LinkedTo.Add(nodePair[0]);
        nodePair[1].LinkedFrom.Add(nodePair[0]);
        await applicationContext.SaveChangesAsync();
        return nodePair;
    }

    public async Task<List<NavNodeEntity>> RemoveNavNodeLink(
        string map,
        string tag,
        NavNodeEntityId nodeA,
        NavNodeEntityId nodeB)
    {
        var nodeIdPair = new List<NavNodeEntityId>([nodeA, nodeB]);
        var nodePair = await applicationContext.NavNode
            .Include(node => node.LinkedTo)
            .Include(node => node.LinkedFrom)
            .Where(node
                => node.Map == map &&
                   node.Tag == tag &&
                   nodeIdPair.Contains(node.Id))
            .AsSingleQuery()
            .ToListAsync();
        
        if (nodePair.Count != 2)
        {
            throw new GmodException($"Found {nodePair.Count} node instead of 2 when appending a link.");
        }

        if (nodePair[0].Id == nodePair[1].Id)
        {
            throw new GmodException("Duplicate node found (this should never happen).");
        }
        
        nodePair[0].LinkedTo.Remove(nodePair[1]);
        nodePair[0].LinkedFrom.Remove(nodePair[1]);
        
        nodePair[1].LinkedTo.Remove(nodePair[0]);
        nodePair[1].LinkedFrom.Remove(nodePair[0]);
        await applicationContext.SaveChangesAsync();
        return nodePair;
    }

    public async Task<List<NavNodeEntity>> GetNodes(
        string map,
        string tag)
    {
        return await applicationContext.NavNode
            .Where(node => node.Map == map && node.Tag == tag)
            .Include(node => node.LinkedTo)
            .Include(node => node.LinkedFrom)
            .AsSingleQuery()
            .ToListAsync();
    }
}