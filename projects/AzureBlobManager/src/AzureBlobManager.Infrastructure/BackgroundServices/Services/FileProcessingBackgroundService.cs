using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using AzureBlobManager.Application.Common.Models;
using AzureBlobManager.Infrastructure.FileProcessing.Services;
using AzureBlobManager.Infrastructure.MessageBus.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AzureBlobManager.Infrastructure.BackgroundServices.Services;

public class FileProcessingBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AzureServiceBusSettings _serviceBusSettings;

    private ServiceBusClient _client;
    private ServiceBusProcessor _processor;
    private bool _isProcessing;

    public FileProcessingBackgroundService(
        IServiceProvider serviceProvider,
        IOptions<AzureServiceBusSettings> serviceBusSettings)
    {
        _serviceProvider = serviceProvider;
        _serviceBusSettings = serviceBusSettings.Value;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var clientOptions = new ServiceBusClientOptions
        {
            RetryOptions = new ServiceBusRetryOptions
            {
                Mode = ServiceBusRetryMode.Exponential,
                MaxRetries = 3,
                MaxDelay = TimeSpan.FromSeconds(10)
            }
        };

        _client = new ServiceBusClient(_serviceBusSettings.ConnectionString, clientOptions);

        var processorOptions = new ServiceBusProcessorOptions
        {
            PrefetchCount = 10,
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 5,
            ReceiveMode = ServiceBusReceiveMode.PeekLock,
            MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(5)
        };

        _processor = _client.CreateProcessor(_serviceBusSettings.QueueName, processorOptions);

        _processor.ProcessMessageAsync += ProcessMessageAsync;
        _processor.ProcessErrorAsync += ProcessErrorAsync;

        await _processor.StartProcessingAsync(cancellationToken);
        _isProcessing = true;

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested && _isProcessing)
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }
        catch
        {
            _isProcessing = false;
            throw;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _isProcessing = false;

        if (_processor != null)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            await _processor.DisposeAsync();
        }

        if (_client != null)
        {
            await _client.DisposeAsync();
        }

        await base.StopAsync(cancellationToken);
    }
    
    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        try
        {
            string messageBody = Encoding.UTF8.GetString(args.Message.Body);

            var message = JsonSerializer.Deserialize<FileUploadMessage>(messageBody);
            if (message == null)
            {
                await args.CompleteMessageAsync(args.Message);
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var fileProcessingService = scope.ServiceProvider.GetRequiredService<IFileProcessingService>();
                await fileProcessingService.ProcessFileAsync(message);
            }

            await args.CompleteMessageAsync(args.Message);
        }
        catch
        {
            await args.AbandonMessageAsync(args.Message);
        }
    }
    
    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        return Task.CompletedTask;
    }
}