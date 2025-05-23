using AutoMapper;
using AzureBlobManager.Application.Commands;
using AzureBlobManager.Application.Common.Dependencies.DataAccess;
using AzureBlobManager.Application.Common.Dependencies.Services;
using MediatR;

namespace AzureBlobManager.Application.Handlers;

public class GetUserFilesQueryHandler : IRequestHandler<GetUserFilesQuery, IEnumerable<UserFileDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetUserFilesQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<UserFileDto>> Handle(GetUserFilesQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var files = await _unitOfWork.UserFileRepository.GetFilesByUserIdAsync(userId);

        return _mapper.Map<IEnumerable<UserFileDto>>(files);
    }
}