namespace AzureBlobManager.Infrastructure.Authentication.Settings;

public class AuthenticationSettings
{
    public string JwtIssuer { get; set; }
    public string JwtAudience { get; set; }
    public byte[] JwtSigningKey { get; private set; }
    public int TokenExpirationSeconds { get; set; }

    public string JwtSigningKeyBase64
    {
        get => _jwtSigningKeyBase64;
        init { _jwtSigningKeyBase64 = value; JwtSigningKey = Convert.FromBase64String(value); }
    }
    private string _jwtSigningKeyBase64;
}