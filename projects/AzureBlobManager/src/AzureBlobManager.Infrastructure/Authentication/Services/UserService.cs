using AzureBlobManager.Domain.Entities;
using AzureBlobManager.Infrastructure.Authentication.Models;
using Microsoft.AspNetCore.Identity;

namespace AzureBlobManager.Infrastructure.Authentication.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public UserService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<(RegisterResult, LoginData?)> RegisterAsync(string username, string email, string password)
    {
        var existingUserByUsername = await _userManager.FindByNameAsync(username);
        if (existingUserByUsername != null)
        {
            return (RegisterResult.UsernameTaken, null);
        }

        var existingUserByEmail = await _userManager.FindByEmailAsync(email);
        if (existingUserByEmail != null)
        {
            return (RegisterResult.EmailTaken, null);
        }

        var user = new User
        {
            UserName = username,
            Email = email,
            EmailConfirmed = false
        };

        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            return (RegisterResult.Failed, null);
        }

        var token = _tokenService.GenerateToken(user.Id, username);
        return (
            RegisterResult.Success,
            new LoginData
            {
                Username = user.UserName,
                Email = user.Email,
                Token = token
            }
        );
}

    public async Task<(LoginResult, LoginData?)> LoginAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return (LoginResult.Failed, null);
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, true);
        if (!signInResult.Succeeded)
        {
            if (signInResult.IsLockedOut)
                return (LoginResult.LockedOut, null);
            if (signInResult.IsNotAllowed)
                return (LoginResult.NotAllowed, null);
            else
                throw new Exception("Unhandled login result");
        }

        var tokenResult = _tokenService.GenerateToken(user.Id, username);
        return new(
            LoginResult.Success,
            new LoginData
            {
                Username = user.UserName!,
                Email = user.Email!,
                Token = tokenResult
            }
        );
    }
}