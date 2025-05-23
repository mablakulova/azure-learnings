using MediatR;

namespace AzureBlobManager.Application.Commands;

public class GetUserFileByIdQuery : IRequest<UserFileDto?>
{
    public Guid FileId { get; set; }
}