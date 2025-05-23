using AzureBlobManager.Infrastructure.Common.Helpers;
using AzureBlobManager.WebApi.CORS.Settings;

namespace AzureBlobManager.WebApi.CORS;

public static class CorsExtensions
{
    public static void AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSettings = configuration.GetOptions<CorsSettings>();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .WithOrigins(corsSettings.AllowedOrigins)
                .Build();
            });
        });
    }

    public static void UseCorsConfiguration(this IApplicationBuilder app)
    {
        app.UseCors();
    }
}