using Microsoft.AspNetCore.Mvc;
using Presentation.Dto.Nav;
using Presentation.Handler;

namespace Api.Endpoints;

public static class NavEndpoints
{
    public static IEndpointRouteBuilder MapNavEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var navGroup = endpoints
            .MapGroup("Nav")
            .WithTags("Nav");
        
        navGroup.MapPost(string.Empty, (
            [FromServices] INavHandler handler,
            [FromBody] CreateNavNodeDto createNavNodeDto) => handler.AddNavNode(createNavNodeDto));
        
        navGroup.MapDelete(string.Empty, (
            [FromServices] INavHandler handler,
            [FromBody] RemoveNavNodeDto removeNavNodeDto) => handler.RemoveNavNode(removeNavNodeDto));
        
        navGroup.MapPost("link", (
            [FromServices] INavHandler handler,
            [FromBody] NavNodeLinkDto navNodeLinkDto) => handler.AppendNavNodeLink(navNodeLinkDto));
        
        navGroup.MapDelete("link", (
            [FromServices] INavHandler handler,
            [FromBody] NavNodeLinkDto navNodeLinkDto) => handler.RemoveNavNodeLink(navNodeLinkDto));
        
        navGroup.MapGet("{map}/{tag}", (
            [FromServices] INavHandler handler,
            [FromRoute] string map,
            [FromRoute] string tag) => handler.GetNodes(map, tag));
        
        // These endpoints break convention on purpose because "starfall" to only allows get and post requests
        navGroup.MapPost("compatibility/create", (
            [FromServices] INavHandler handler,
            [FromBody] CreateNavNodeDto createNavNodeDto) => handler.AddNavNode(createNavNodeDto));
        
        navGroup.MapPost("compatibility/delete", (
            [FromServices] INavHandler handler,
            [FromBody] RemoveNavNodeDto removeNavNodeDto) => handler.RemoveNavNode(removeNavNodeDto));
        
        navGroup.MapPost("compatibility/link/delete", (
            [FromServices] INavHandler handler,
            [FromBody] NavNodeLinkDto navNodeLinkDto) => handler.RemoveNavNodeLink(navNodeLinkDto));

        return navGroup;
    }
}