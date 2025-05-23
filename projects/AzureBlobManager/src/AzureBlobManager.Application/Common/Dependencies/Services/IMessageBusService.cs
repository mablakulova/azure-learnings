using AzureBlobManager.Application.Common.Models;

namespace AzureBlobManager.Application.Common.Dependencies.Services;

public interface IMessageBusService
{
    Task SendFileUploadedMessageAsync(FileUploadMessage message);
}