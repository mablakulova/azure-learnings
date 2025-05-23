using MediatR;
using Microsoft.AspNetCore.Http;

namespace AzureBlobManager.Application.Commands;

public class UploadUserFileCommand : IRequest<UserFileDto>
{
    public IFormFile File { get; set; }
}