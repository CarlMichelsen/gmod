using Api.Extensions;
using Application.Client.Discord;
using Application.Configuration.Options;
using Application.Handler;
using Application.Service;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Presentation.Client.Discord;
using Presentation.Handler;
using Presentation.Service;

namespace Api;

public static class Dependencies
{
    public static void RegisterGmodDependencies(this WebApplicationBuilder builder)
    {
        // Business-logic
        builder.Services
            .AddScoped<IImageService, ImageService>()
            .AddScoped<IImageHandler, ImageHandler>();
        
        // Database
        builder.Services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b =>
                    {
                        b.MigrationsAssembly("Api");
                        b.MigrationsHistoryTable("__EFMigrationsHistory", ApplicationContext.SchemaName);
                    })
                .UseSnakeCaseNamingConvention();
            
            if (builder.Environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });
        
        // Configuration
        builder.Services
            .AddOpenApi()
            .AddConfiguredOpenTelemetry()
            .AddSingleton(TimeProvider.System);
        builder.Configuration.AddJsonFile(
            "secrets.json",
            optional: builder.Environment.IsDevelopment(),
            reloadOnChange: false);
        builder
            .AddValidatedOptions<GmodOptions>();
        builder.ApplicationUseSerilog();
        
        // Middleware
        builder.Services
            .AddHttpContextAccessor()
            .AddCustomProblemDetails();
        
        // HealthChecks
        builder.Services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());
        
        // Cache
        builder.Services
            .AddMemoryCache()
            .AddOutputCache();
        
        // Client
        builder.Services
            .AddHttpClient<IDiscordWebhookMessageClient, DiscordWebhookMessageClient>()
            .AddStandardResilienceHandler();
    }
}