using Microsoft.AspNetCore.Mvc;
using Presentation.Dto.Image;
using Presentation.Handler;

namespace Api.Endpoints;

public static class ImageEndpoints
{
    public static IEndpointRouteBuilder MapImageEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var imageGroup = endpoints
            .MapGroup("Image")
            .WithTags("Image");

        imageGroup.MapPost("Load", (
                [FromServices] IImageHandler handler,
                [FromBody] ImageRequestDto requestDto) =>
            handler.LoadImage(requestDto));

        imageGroup.MapGet("{imageId:guid}", (
                [FromServices] IImageHandler handler,
                [FromRoute] Guid imageId) =>
            handler.GetImage(imageId: imageId));
        
        imageGroup.MapGet("List/{skip:int}/{take:int}", (
                [FromServices] IImageHandler handler,
                [FromRoute] int skip,
                [FromRoute] int take) =>
            handler.GetImages(skip, take));
        
        imageGroup.MapGet("{imageId:guid}/{skip:int}/{take:int}", (
                [FromServices] IImageHandler handler,
                [FromRoute] Guid imageId,
                [FromRoute] int skip,
                [FromRoute] int take) =>
            handler.GetPartialImage(imageId, skip, take));
        
        imageGroup.MapDelete("{imageId:guid}", (
                [FromServices] IImageHandler handler,
                [FromRoute] Guid imageId) =>
            handler.DeleteImage(imageId));
        
        // These endpoints break convention on purpose because 'expression 2' to only allow get requests
        imageGroup.MapGet("compatibility/Load", (
                [FromServices] IImageHandler handler,
                [AsParameters] ImageRequestDto requestDto) =>
            handler.LoadImage(requestDto));
        
        imageGroup.MapGet("compatibility/delete/{imageId:guid}", (
                [FromServices] IImageHandler handler,
                [FromRoute] Guid imageId) =>
            handler.DeleteImage(imageId));
        
        return imageGroup;
    }
}