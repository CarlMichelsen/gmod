using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class RootEndpoints
{
    public static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health");
        
        var apiGroup = endpoints
            .MapGroup("api/v1")
            .WithParameterValidation();
        
        apiGroup.MapImageEndpoints();
        
        apiGroup.MapNavEndpoints();

        return endpoints;
    }
}