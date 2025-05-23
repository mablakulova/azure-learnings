using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AzureBlobManager.Infrastructure.Authentication.Models;
using AzureBlobManager.Infrastructure.Authentication.Settings;
using Microsoft.IdentityModel.Tokens;

namespace AzureBlobManager.Infrastructure.Authentication.Services;

public class JwtTokenService : ITokenService
{
    private readonly AuthenticationSettings _authSettings;

    public JwtTokenService(AuthenticationSettings authSettings)
    {
        _authSettings = authSettings;
    }

    public TokenData GenerateToken(int userId, string userName)
    {
        var expiration = DateTime.UtcNow.AddSeconds(_authSettings.TokenExpirationSeconds);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, _authSettings.JwtIssuer),
                new Claim(JwtRegisteredClaimNames.Aud, _authSettings.JwtAudience),
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName)
            }),
            Expires = expiration,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_authSettings.JwtSigningKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new TokenData
        {
            TokenType = "Bearer",
            AccessToken = tokenString,
            ExpiresAt = expiration
        };
    }
}