using AzureBlobManager.Infrastructure.FileProcessing.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureBlobManager.Infrastructure.FileProcessing;

public static class FileProcessingExtensions
{
    public static IServiceCollection AddFileProcessingServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IFileProcessingService, FileProcessingService>();
        return services;
    }
}