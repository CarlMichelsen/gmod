using Microsoft.AspNetCore.Mvc;
using Presentation.Dto.Odometer;
using Presentation.Handler;

namespace Api.Endpoints;

public static class OdometerEndpoints
{
    public static IEndpointRouteBuilder MapOdometerEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var odometerGroup = endpoints
            .MapGroup("Odometer")
            .WithTags("Odometer");
        
        odometerGroup.MapPost("{map}/{tag}", (
                [FromServices] IOdometerHandler handler,
                [FromRoute] string map,
                [FromRoute] string tag) =>
            handler.StartNewTrip(map, tag));
        
        odometerGroup.MapPost("{map}/{tag}/{tripId:guid}", (
                [FromServices] IOdometerHandler handler,
                [FromBody] OdometerChunkDto chunk,
                [FromRoute] string map,
                [FromRoute] string tag,
                [FromRoute] Guid tripId) =>
            handler.AppendTrip(map, tag, tripId, chunk));
        
        odometerGroup.MapGet("{map}/{tag}", (
                [FromServices] IOdometerHandler handler,
                [FromRoute] string map,
                [FromRoute] string tag) =>
            handler.GetOdometerData(map, tag));
        
        return odometerGroup;
    }
}