using AzureBlobManager.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobManager.WebApi.API.V1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/userFiles/[action]")]
public class UserFilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserFilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<UserFileDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserFileDto>>> GetUserFiles([FromBody] GetUserFilesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserFileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserFileDto>> GetUserFile(Guid id)
    {
        var result = await _mediator.Send(new GetUserFileByIdQuery { FileId = id });
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserFileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserFileDto>> UploadUserFile(IFormFile file)
    {
        var result = await _mediator.Send(new UploadUserFileCommand { File = file });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> DeleteUserFile(Guid id)
    {
        var result = await _mediator.Send(new DeleteUserFileCommand { FileId = id });
        return Ok(result);
    }
}