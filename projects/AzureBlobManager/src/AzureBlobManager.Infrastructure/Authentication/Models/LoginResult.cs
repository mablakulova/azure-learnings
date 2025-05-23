namespace AzureBlobManager.Infrastructure.Authentication.Models;

public enum LoginResult
{
    Success,
    LockedOut,
    NotAllowed,
    Failed
}