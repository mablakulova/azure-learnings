namespace AzureBlobManager.Infrastructure.Authentication.Models;

public class TokenData
{
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }

    public int GetRemainTime()
    {
        return Math.Max(0, (int)(ExpiresAt - DateTime.Now).TotalSeconds);
    }
}