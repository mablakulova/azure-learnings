using System.Text.Json;

namespace AzureBlobManager.WebApi.API;

internal static class ApiExtensions
{
    public static void AddApi(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.AddControllers()
           .AddControllersAsServices()
           .AddJsonOptions(c =>
               c.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
    }

    public static void UseApi(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });
    }
}