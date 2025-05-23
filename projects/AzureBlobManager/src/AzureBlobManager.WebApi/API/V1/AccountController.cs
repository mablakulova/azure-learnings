using AzureBlobManager.Infrastructure.Authentication.Models;
using AzureBlobManager.Infrastructure.Authentication.Services;
using AzureBlobManager.WebApi.Authentication.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobManager.WebApi.API.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("v{v:apiVersion}/account/[action]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(LoginData), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginData>> Register([FromBody] RegisterDto dto)
    {
        var (result, data) = await _userService.RegisterAsync(dto.Username, dto.Email, dto.Password);
        return result switch
        {
            RegisterResult.UsernameTaken => BadRequest(new { message = "Username is already taken" }),
            RegisterResult.EmailTaken => BadRequest(new { message = "Email is already registered" }),
            RegisterResult.Failed => BadRequest(new { message = "Registration failed. Please try again." }),
            RegisterResult.Success => Ok(data),
            _ => StatusCode(500, new { message = "An unexpected error occurred" })
        };
    }

    [HttpPost]
    [ProducesResponseType(typeof(LoginData), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginData>> Login([FromBody] LoginDto dto)
    {
        var (result, data) = await _userService.LoginAsync(dto.Username, dto.Password);
        return result switch
        {
            LoginResult.Failed => Unauthorized("Username or password incorrect"),
            LoginResult.LockedOut => Forbid("User is temporarily locked out."),
            LoginResult.NotAllowed => Forbid("User is not allowed to sign in."),
            LoginResult.Success => Ok(data),
            _ => StatusCode(500, new { message = "An unexpected error occurred" })
        };
    }

    [ApiVersionNeutral]
    [HttpPost("/account/oauth2/access_token")]
    public async Task<ActionResult<dynamic>> LoginForm([FromForm] LoginDto dto)
    {
        var (result, data) = await _userService.LoginAsync(dto.Username, dto.Password);
        return result switch
        {
           LoginResult.Success => Ok(new
           {
               access_token = data!.Token.AccessToken,
               token_type = data.Token.TokenType,
               expires_in = data.Token.GetRemainTime()
            }),
            _ => Unauthorized()
        };
    }
}