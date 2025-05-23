using AzureBlobManager.Application.Common.Dependencies.DataAccess;
using AzureBlobManager.Infrastructure.Persistence.Context;

namespace AzureBlobManager.Infrastructure.Common.Dependencies.DataAccess;

internal class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public IUserFileRepository UserFileRepository { get; }

    public UnitOfWork(
        ApplicationDbContext dbContext,
        IUserFileRepository userFileRepository)
    {
        _dbContext = dbContext;
        UserFileRepository = userFileRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}