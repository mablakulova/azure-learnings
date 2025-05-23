using AzureBlobManager.Application.Commands;
using AzureBlobManager.Application.Common.Dependencies.DataAccess;
using AzureBlobManager.Application.Common.Dependencies.Services;
using MediatR;

namespace AzureBlobManager.Application.Handlers;

public class DeleteFileCommandHandler : IRequestHandler<DeleteUserFileCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorage;

    public DeleteFileCommandHandler(
        IUnitOfWork unitOfWork,
        IBlobStorageService blobStorage)
    {
        _unitOfWork = unitOfWork;
        _blobStorage = blobStorage;
    }

    public async Task<bool> Handle(DeleteUserFileCommand request, CancellationToken cancellationToken)
    {
        var file = await _unitOfWork.UserFileRepository.GetByIdAsync(request.FileId);
        if (file == null)
           return false;

        await _blobStorage.DeleteFileAsync(file.Id.ToString());
                
        await _unitOfWork.UserFileRepository.DeleteAsync(file);
        await _unitOfWork.SaveChangesAsync();
            
        return true;
    }
}