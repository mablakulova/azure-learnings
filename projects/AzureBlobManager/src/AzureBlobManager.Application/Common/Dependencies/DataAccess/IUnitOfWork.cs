namespace AzureBlobManager.Application.Common.Dependencies.DataAccess;

public interface IUnitOfWork
{
    IUserFileRepository UserFileRepository { get; }
    Task<int> SaveChangesAsync();
}