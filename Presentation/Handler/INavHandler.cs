using Presentation.Dto;
using Presentation.Dto.Nav;

namespace Presentation.Handler;

public interface INavHandler
{
    /// <summary>
    /// Add nav node.
    /// </summary>
    /// <param name="createNavNodeDto">Create nav node dto.</param>
    /// <returns>Affected NavNodes including the newly created one.</returns>
    Task<ServiceResponse<List<NavNodeDto>>> AddNavNode(CreateNavNodeDto createNavNodeDto);
    
    /// <summary>
    /// Remove nad node.
    /// </summary>
    /// <param name="removeNavNodeDto">Remove nav node dto.</param>
    /// <returns>Affected nodes excluding the removed node.</returns>
    Task<ServiceResponse<List<NavNodeDto>>> RemoveNavNode(RemoveNavNodeDto removeNavNodeDto);
    
    /// <summary>
    /// Append two-way link to an existing nav-node in a tag/map group.
    /// </summary>
    /// <param name="navNodeLinkDto">Append nav-node link dto.</param>
    /// <returns>Affected NavNodes.</returns>
    Task<ServiceResponse<List<NavNodeDto>>> AppendNavNodeLink(NavNodeLinkDto navNodeLinkDto);

    /// <summary>
    /// Remove two-way link from a tag/map group.
    /// </summary>
    /// <param name="navNodeLinkDto">Remove nav-node link dto.</param>
    /// <returns>Affected NavNodes.</returns>
    Task<ServiceResponse<List<NavNodeDto>>> RemoveNavNodeLink(NavNodeLinkDto navNodeLinkDto);
    
    /// <summary>
    /// Get nodes from tag/map group.
    /// </summary>
    /// <param name="map">What map does the nodes exist on.</param>
    /// <param name="tag">What tag are the nodes from.</param>
    /// <returns>All nodes from tag/map group.</returns>
    Task<ServiceResponse<List<NavNodeDto>>> GetNodes(string map, string tag);
}