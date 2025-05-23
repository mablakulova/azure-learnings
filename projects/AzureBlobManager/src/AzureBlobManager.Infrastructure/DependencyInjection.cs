using Azure.Identity;
using AzureBlobManager.Application.Common.Dependencies.DataAccess;
using AzureBlobManager.Infrastructure.Authentication;
using AzureBlobManager.Infrastructure.BackgroundServices;
using AzureBlobManager.Infrastructure.BlobStorage;
using AzureBlobManager.Infrastructure.Common.Dependencies.DataAccess;
using AzureBlobManager.Infrastructure.Common.Helpers;
using AzureBlobManager.Infrastructure.FileProcessing;
using AzureBlobManager.Infrastructure.MessageBus;
using AzureBlobManager.Infrastructure.Persistence.Context;
using AzureBlobManager.Infrastructure.Persistence.Settings;
using AzureBlobManager.Infrastructure.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureBlobManager.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureConfiguration(this IConfigurationBuilder configBuilder)
    {
        var settings = configBuilder.Build().GetOptions<AzureKeyVaultSettings>();

        if (settings is not null && settings.AddToConfiguration)
        {
            configBuilder.AddAzureKeyVault(new Uri(settings.ServiceUrl), new DefaultAzureCredential());
        }
    }

    public static IServiceCollection AddInfrastructureDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddIdentityServices(configuration)
            .AddAuthServices(configuration)
            .AddDbServices(configuration)
            .AddBlobStorageServices(configuration)
            .AddFileProcessingServices(configuration)
            .AddMessageBusServices(configuration)
            .AddBackgroundServices(configuration)
            .AddApplicationServices(configuration);

        return services;
    }

    public static void UseInfrastructure(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    public static IServiceCollection AddDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetOptions<DbConnectionStrings>().DefaultConnection,
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
                
        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserFileRepository, UserFileRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}