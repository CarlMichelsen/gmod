using Database.Entity;
using Database.Entity.Id;

namespace Presentation.Service;

public interface INavService
{
    Task<List<NavNodeEntity>> AddNavNode(
        string map,
        string tag,
        int x,
        int y,
        int z,
        List<NavNodeEntityId> links);
    
    Task<List<NavNodeEntity>> RemoveNavNode(
        string map,
        string tag,
        NavNodeEntityId id);
    
    Task<List<NavNodeEntity>> AppendNavNodeLink(
        string map,
        string tag,
        NavNodeEntityId nodeA,
        NavNodeEntityId nodeB);
    
    Task<List<NavNodeEntity>> RemoveNavNodeLink(
        string map,
        string tag,
        NavNodeEntityId nodeA,
        NavNodeEntityId nodeB);
    
    Task<List<NavNodeEntity>> GetNodes(
        string map,
        string tag);
}