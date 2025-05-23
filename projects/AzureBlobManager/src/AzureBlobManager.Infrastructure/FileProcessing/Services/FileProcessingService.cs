using AzureBlobManager.Application.Common.Dependencies.DataAccess;
using AzureBlobManager.Application.Common.Dependencies.Services;
using AzureBlobManager.Application.Common.Models;

namespace AzureBlobManager.Infrastructure.FileProcessing.Services;

public class FileProcessingService : IFileProcessingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public FileProcessingService(
        IUnitOfWork unitOfWork,
        IBlobStorageService blobStorageService)
    {
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task ProcessFileAsync(FileUploadMessage message)
    {
        var file = await _unitOfWork.UserFileRepository.GetByIdAsync(message.FileId);
        if (file == null)
            return;

        bool isValid = false;
        string? fileHash = null;

        await using (var stream = await _blobStorageService.DownloadFileAsync(message.BlobName))
        {
            if (stream.Length > 0)
            {
                isValid = true;
                fileHash = await GetFileHashAsync(stream);
            }
        }

        if (isValid)
        {
            file.IsProcessed = true;
            file.Hash = fileHash;
            await _unitOfWork.UserFileRepository.UpdateAsync(file);
        }
        else
        {
            await _blobStorageService.DeleteFileAsync(file.BlobName);
            await _unitOfWork.UserFileRepository.DeleteAsync(file);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<string> GetFileHashAsync(Stream stream)
    {
        if (!stream.CanSeek)
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            stream = memoryStream;
        }
        else
        {
            stream.Position = 0;
        }

        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hash = await sha256.ComputeHashAsync(stream);

        return Convert.ToHexString(hash);
    }
}