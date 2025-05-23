using System.Text.Json;
using Azure.Messaging.ServiceBus;
using AzureBlobManager.Application.Common.Dependencies.Services;
using AzureBlobManager.Application.Common.Models;
using AzureBlobManager.Infrastructure.MessageBus.Settings;
using Microsoft.Extensions.Options;

namespace AzureBlobManager.Infrastructure.MessageBus.Services;

public class AzureServiceBusService : IMessageBusService, IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;
    private readonly string _queueName;

    public AzureServiceBusService(IOptions<AzureServiceBusSettings> settings)
    {
        var serviceBusSettings = settings.Value;
        _queueName = serviceBusSettings.QueueName;

        var clientOptions = new ServiceBusClientOptions
        {
            RetryOptions = new ServiceBusRetryOptions
            {
                Mode = ServiceBusRetryMode.Exponential,
                MaxRetries = 3,
                MaxDelay = TimeSpan.FromSeconds(10)
            }
        };
        
        _client = new ServiceBusClient(serviceBusSettings.ConnectionString, clientOptions);
        _sender = _client.CreateSender(_queueName);
    }

    public async Task SendFileUploadedMessageAsync(FileUploadMessage message)
    {
        var messageBody = JsonSerializer.Serialize(message);

        var serviceBusMessage = new ServiceBusMessage(messageBody)
        {
            ContentType = "application/json",
            Subject = "FileUploaded",
            MessageId = Guid.NewGuid().ToString(),
            ApplicationProperties = 
            {
                { "FileId", message.FileId.ToString() },
                { "UserId", message.UserId.ToString() }
            }
        };

        await _sender.SendMessageAsync(serviceBusMessage);
    }

    public async ValueTask DisposeAsync()
    {
        if (_sender != null)
        {
            await _sender.DisposeAsync();
        }

        if (_client != null)
        {
            await _client.DisposeAsync();
        }
    }
}