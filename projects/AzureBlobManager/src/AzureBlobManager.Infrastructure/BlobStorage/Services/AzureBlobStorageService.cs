using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobManager.Application.Common.Dependencies.Services;
using AzureBlobManager.Infrastructure.BlobStorage.Settings;
using Microsoft.Extensions.Options;

namespace AzureBlobManager.Infrastructure.BlobStorage.Services;

public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public AzureBlobStorageService(
        BlobServiceClient blobServiceClient,
        IOptions<AzureBlobStorageSettings> settings)
    {
        _blobServiceClient = blobServiceClient;
        _containerName = settings.Value.ContainerName;
    }

    public async Task<(string fileId, string blobUrl)> UploadFileAsync(Stream fileStream, string contentType)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        var fileId = Guid.NewGuid().ToString();
        var blobClient = containerClient.GetBlobClient(blobName: fileId);
        await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });

        return (fileId, blobClient.Uri.ToString());
    }

    public async Task<Stream> DownloadFileAsync(string fileId)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName: fileId);

        var downloadInfo = await blobClient.DownloadAsync();
        return downloadInfo.Value.Content;
    }

    public async Task<bool> DeleteFileAsync(string fileId)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName: fileId);

        return await blobClient.DeleteIfExistsAsync();
    }
}