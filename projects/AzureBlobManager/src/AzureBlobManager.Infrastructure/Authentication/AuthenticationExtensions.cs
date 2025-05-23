using System.IdentityModel.Tokens.Jwt;
using AzureBlobManager.Domain.Entities;
using AzureBlobManager.Infrastructure.Authentication.Services;
using AzureBlobManager.Infrastructure.Authentication.Settings;
using AzureBlobManager.Infrastructure.Common.Helpers;
using AzureBlobManager.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AzureBlobManager.Infrastructure.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, IdentityRole<int>>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
        })
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }
    
    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.RegisterOptions<AuthenticationSettings>();

        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        var authSettings = configuration.GetOptions<AuthenticationSettings>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authSettings.JwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = authSettings.JwtIssuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(authSettings.JwtSigningKey),
                    ClockSkew = TimeSpan.FromMinutes(5),
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
            });

        return services;
    }
}