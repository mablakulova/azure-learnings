using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AzureBlobManager.Application.Common.Dependencies.Services;

namespace AzureBlobManager.WebApi.Authentication.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.User != null)
        {
            var userIdClaim = httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!string.IsNullOrEmpty(userIdClaim))
            {
                UserId = int.Parse(userIdClaim);
            }
        }
    }

    public int UserId { get; }
}