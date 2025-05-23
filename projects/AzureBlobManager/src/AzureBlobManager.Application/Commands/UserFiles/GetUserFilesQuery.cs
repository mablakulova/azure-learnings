using MediatR;

namespace AzureBlobManager.Application.Commands;

public class GetUserFilesQuery : IRequest<IEnumerable<UserFileDto>>
{
}