using Application.Mapper;
using Database.Entity.Id;
using Microsoft.Extensions.Logging;
using Presentation.Dto;
using Presentation.Dto.Nav;
using Presentation.Handler;
using Presentation.Service;
using Presentation.Service.Exception;

namespace Application.Handler;

public class NavHandler(
    ILogger<NavHandler> logger,
    INavService navService) : INavHandler
{
    public async Task<ServiceResponse<List<NavNodeDto>>> AddNavNode(CreateNavNodeDto createNavNodeDto)
    {
        try
        {
            var links = createNavNodeDto.LinkedNodes
                .Select(x => new NavNodeEntityId(x))
                .ToList();
            var newAndAffectedNodes = await navService.AddNavNode(
                map: createNavNodeDto.Map,
                tag: createNavNodeDto.Tag,
                x: createNavNodeDto.X,
                y: createNavNodeDto.Y,
                z: createNavNodeDto.Z,
                links: links);

            logger.LogInformation(
                "Added nav node for map '{Map}' with tag '{Tag}' [{X}, {Y}, {Z}]",
                createNavNodeDto.Map,
                createNavNodeDto.Tag,
                createNavNodeDto.X,
                createNavNodeDto.Y,
                createNavNodeDto.Z);
            
            var dto = newAndAffectedNodes
                .Select(node => node.ToDto())
                .ToList();
            return new ServiceResponse<List<NavNodeDto>>(dto);
        }
        catch (GmodException e)
        {
            return new ServiceResponse<List<NavNodeDto>>(e.Message);
        }
    }

    public async Task<ServiceResponse<List<NavNodeDto>>> RemoveNavNode(RemoveNavNodeDto removeNavNodeDto)
    {
        try
        {
            var affectedNodesExcludingRemovedNode = await navService
                .RemoveNavNode(
                    map: removeNavNodeDto.Map,
                    tag: removeNavNodeDto.Tag,
                    id: new NavNodeEntityId(removeNavNodeDto.Id));
            
            logger.LogInformation(
                "Removed nav node for map '{Map}' with tag '{Tag}' - '{NodeId}'",
                removeNavNodeDto.Map,
                removeNavNodeDto.Tag,
                removeNavNodeDto.Id);
            
            var dto = affectedNodesExcludingRemovedNode
                .Select(node => node.ToDto())
                .ToList();
            return new ServiceResponse<List<NavNodeDto>>(dto);
        }
        catch (GmodException e)
        {
            return new ServiceResponse<List<NavNodeDto>>(e.Message);
        }
    }

    public async Task<ServiceResponse<List<NavNodeDto>>> AppendNavNodeLink(NavNodeLinkDto navNodeLinkDto)
    {
        try
        {
            var nodes = await navService.AppendNavNodeLink(
                map: navNodeLinkDto.Map,
                tag: navNodeLinkDto.Tag,
                nodeA: new NavNodeEntityId(navNodeLinkDto.NodeAId),
                nodeB: new NavNodeEntityId(navNodeLinkDto.NodeBId));
            
            logger.LogInformation(
                "Added link for map '{Map}' with tag '{Tag}' between nav nodes '{NodeA}' and '{NodeB}'",
                navNodeLinkDto.Map,
                navNodeLinkDto.Tag,
                navNodeLinkDto.NodeAId,
                navNodeLinkDto.NodeBId);
            
            var dto = nodes
                .Select(node => node.ToDto())
                .ToList();
            return new ServiceResponse<List<NavNodeDto>>(dto);
        }
        catch (GmodException e)
        {
            return new ServiceResponse<List<NavNodeDto>>(e.Message);
        }
    }

    public async Task<ServiceResponse<List<NavNodeDto>>> RemoveNavNodeLink(NavNodeLinkDto navNodeLinkDto)
    {
        try
        {
            var nodes = await navService.RemoveNavNodeLink(
                map: navNodeLinkDto.Map,
                tag: navNodeLinkDto.Tag,
                nodeA: new NavNodeEntityId(navNodeLinkDto.NodeAId),
                nodeB: new NavNodeEntityId(navNodeLinkDto.NodeBId));
            
            logger.LogInformation(
                "Removed link for map '{Map}' with tag '{Tag}' between nav nodes '{NodeA}' and '{NodeB}'",
                navNodeLinkDto.Map,
                navNodeLinkDto.Tag,
                navNodeLinkDto.NodeAId,
                navNodeLinkDto.NodeBId);
            
            var dto = nodes
                .Select(node => node.ToDto())
                .ToList();
            return new ServiceResponse<List<NavNodeDto>>(dto);
        }
        catch (GmodException e)
        {
            return new ServiceResponse<List<NavNodeDto>>(e.Message);
        }
    }

    public async Task<ServiceResponse<List<NavNodeDto>>> GetNodes(string map, string tag)
    {
        try
        {
            var nodes = await navService
                .GetNodes(map: map, tag: tag);
            
            var dto = nodes
                .Select(node => node.ToDto())
                .ToList();
            return new ServiceResponse<List<NavNodeDto>>(dto);
        }
        catch (GmodException e)
        {
            return new ServiceResponse<List<NavNodeDto>>(e.Message);
        }
    }
}