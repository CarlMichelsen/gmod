using Scalar.AspNetCore;

namespace Api.Extensions;

public static class OpenApiExtensions
{
    public static WebApplication MapOpenApiAndScalar(this WebApplication app)
    {
        app
            .MapOpenApi()
            .CacheOutput();
    
        app
            .MapScalarApiReference()
            .CacheOutput();

        return app;
    }
}