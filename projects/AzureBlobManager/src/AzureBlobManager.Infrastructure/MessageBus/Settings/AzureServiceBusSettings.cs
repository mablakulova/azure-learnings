namespace AzureBlobManager.Infrastructure.MessageBus.Settings;

public class AzureServiceBusSettings
{
    public string ConnectionString { get; set; }
    public string QueueName { get; set; }
}