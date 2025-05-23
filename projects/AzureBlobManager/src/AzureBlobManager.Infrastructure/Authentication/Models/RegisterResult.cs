namespace AzureBlobManager.Infrastructure.Authentication.Models;

public enum RegisterResult
{
    Success,
    EmailTaken,
    UsernameTaken,
    Failed
}