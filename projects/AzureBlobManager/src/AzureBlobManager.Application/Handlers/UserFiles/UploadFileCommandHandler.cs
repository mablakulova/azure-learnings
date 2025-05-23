using AutoMapper;
using AzureBlobManager.Application.Commands;
using AzureBlobManager.Application.Common.Dependencies.DataAccess;
using AzureBlobManager.Application.Common.Dependencies.Services;
using AzureBlobManager.Application.Common.Models;
using AzureBlobManager.Domain.Entities;
using MediatR;

namespace AzureBlobManager.Application.Handlers;

public class UploadFileCommandHandler : IRequestHandler<UploadUserFileCommand, UserFileDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IMessageBusService _messageBusService;

    public UploadFileCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IBlobStorageService blobStorageService,
        IMessageBusService messageBusService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _blobStorageService = blobStorageService;
        _messageBusService = messageBusService;
    }

    public async Task<UserFileDto> Handle(UploadUserFileCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        using var stream = request.File.OpenReadStream();
        var (fileId, blobUrl) = await _blobStorageService.UploadFileAsync(stream, request.File.ContentType);
                
        var userFile = new UserFile
        {
            Id = new Guid(fileId),
            FileName = request.File.FileName,
            ContentType = request.File.ContentType,
            Size = request.File.Length,
            BlobName = fileId,
            BlobUri = blobUrl,
            UploadedAt = DateTime.Now,
            IsProcessed = false,
            UserId = userId
        };
                
        await _unitOfWork.UserFileRepository.AddAsync(userFile);
        await _unitOfWork.SaveChangesAsync();
                
        var message = new FileUploadMessage
        {
            FileId = userFile.Id,
            UserId = userId,
            BlobName = userFile.BlobName,
            ContentType = userFile.ContentType
        };
        await _messageBusService.SendFileUploadedMessageAsync(message);

        return _mapper.Map<UserFileDto>(userFile);
    }
}