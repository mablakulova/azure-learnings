using AzureBlobManager.Application.Common.Models;

namespace AzureBlobManager.Infrastructure.FileProcessing.Services;

public interface IFileProcessingService
{
    Task ProcessFileAsync(FileUploadMessage message);
}