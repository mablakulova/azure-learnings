using AzureBlobManager.Application.Common.Dependencies.Services;
using AzureBlobManager.Infrastructure.Common.Helpers;
using AzureBlobManager.Infrastructure.MessageBus.Services;
using AzureBlobManager.Infrastructure.MessageBus.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureBlobManager.Infrastructure.MessageBus;

public static class MessageBusExtensions
{
    public static IServiceCollection AddMessageBusServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterOptions<AzureServiceBusSettings>();

        services.AddSingleton<IMessageBusService, AzureServiceBusService>();
        return services;
    }
}