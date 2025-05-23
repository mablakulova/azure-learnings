using AzureBlobManager.Domain.Entities;

namespace AzureBlobManager.Application.Common.Dependencies.DataAccess;

public interface IUserFileRepository : IRepository<UserFile, Guid>
{
    Task<IEnumerable<UserFile>> GetFilesByUserIdAsync(int userId);
}