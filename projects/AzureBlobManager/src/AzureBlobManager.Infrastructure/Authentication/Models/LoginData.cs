namespace AzureBlobManager.Infrastructure.Authentication.Models;

public class LoginData
{
    public string Username { get; set; }
    public string Email { get; set; }
    public TokenData Token { get; set; }
}