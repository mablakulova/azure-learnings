using AzureBlobManager.Infrastructure.Authentication.Models;

namespace AzureBlobManager.Infrastructure.Authentication.Services;

public interface ITokenService
{
    TokenData GenerateToken(int userId, string userName);
}