namespace AzureBlobManager.Application.Common.Dependencies.Services;

public interface ICurrentUserService
{
    int UserId { get; }
}