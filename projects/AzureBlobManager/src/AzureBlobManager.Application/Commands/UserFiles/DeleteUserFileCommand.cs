using MediatR;

namespace AzureBlobManager.Application.Commands;

public class DeleteUserFileCommand : IRequest<bool>
{
    public Guid FileId { get; set; }
}