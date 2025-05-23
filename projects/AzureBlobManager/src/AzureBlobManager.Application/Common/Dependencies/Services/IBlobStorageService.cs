namespace AzureBlobManager.Application.Common.Dependencies.Services;

public interface IBlobStorageService
{
    Task<(string fileId, string blobUrl)> UploadFileAsync(Stream fileStream, string contentType);
    Task<Stream> DownloadFileAsync(string fileId);
    Task<bool> DeleteFileAsync(string fileId);
}