using AzureBlobManager.Application.Common.Dependencies.Services;
using AzureBlobManager.WebApi.Authentication.Services;

namespace AzureBlobManager.WebApi.Authentication;

internal static class AuthenticationExtensions
{
    public static void AddAuthUserServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}