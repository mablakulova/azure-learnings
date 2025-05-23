using AzureBlobManager.Infrastructure.Authentication.Models;

namespace AzureBlobManager.Infrastructure.Authentication.Services;

public interface IUserService
{
    Task<(RegisterResult, LoginData?)> RegisterAsync(string username, string email, string password);
    Task<(LoginResult, LoginData?)> LoginAsync(string username, string password);
}