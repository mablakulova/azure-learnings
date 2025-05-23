using AzureBlobManager.Infrastructure.BackgroundServices.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureBlobManager.Infrastructure.BackgroundServices;

public static class BackgroundExtensions
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<FileProcessingBackgroundService>();
        
        return services;
    }
}