using AutoMapper;
using AzureBlobManager.Application.Commands;
using AzureBlobManager.Application.Common.Dependencies.DataAccess;
using AzureBlobManager.Application.Common.Dependencies.Services;
using MediatR;

namespace AzureBlobManager.Application.Handlers;

public class GetFileByIdQueryHandler : IRequestHandler<GetUserFileByIdQuery, UserFileDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetFileByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserFileDto?> Handle(GetUserFileByIdQuery request, CancellationToken cancellationToken)
    {
        var file = await _unitOfWork.UserFileRepository.GetByIdAsync(request.FileId);
        if (file == null)
           return null;

        return _mapper.Map<UserFileDto>(file);
    }
}