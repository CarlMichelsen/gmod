using Api;
using Api.Endpoints;
using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Dependencies
builder.RegisterGmodDependencies();

var app = builder.Build();

if (builder.Environment.IsProduction())
{
    // "Who throws a shoe? Honestly!"
    await app.Services.EnsureDatabaseUpdated();
}

// OpenApi and Scalar endpoints - only enabled in development mode
app.MapOpenApiAndScalar();

// Handle uncaught exceptions
app.UseExceptionHandler();

// Application endpoints
app.MapApplicationEndpoints();

// Output cache
app.UseOutputCache();

app.LogStartup();

app.Run();