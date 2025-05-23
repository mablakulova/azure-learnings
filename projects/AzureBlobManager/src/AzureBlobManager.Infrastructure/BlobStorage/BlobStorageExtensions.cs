using Azure.Storage.Blobs;
using AzureBlobManager.Application.Common.Dependencies.Services;
using AzureBlobManager.Infrastructure.BlobStorage.Services;
using AzureBlobManager.Infrastructure.BlobStorage.Settings;
using AzureBlobManager.Infrastructure.Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureBlobManager.Infrastructure.BlobStorage;

public static class BlobStorageExtensions
{
    public static IServiceCollection AddBlobStorageServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.RegisterOptions<AzureBlobStorageSettings>();

        services.AddSingleton(x => new BlobServiceClient(
            configuration.GetOptions<AzureBlobStorageSettings>().ConnectionString));
            
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
        
        return services;
    }
}