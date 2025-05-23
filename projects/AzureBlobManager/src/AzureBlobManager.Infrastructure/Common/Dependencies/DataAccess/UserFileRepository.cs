using System.Data.Common;
using AzureBlobManager.Application.Common.Dependencies.DataAccess;
using AzureBlobManager.Domain.Entities;
using AzureBlobManager.Infrastructure.Persistence.Context;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace AzureBlobManager.Infrastructure.Common.Dependencies.DataAccess;

internal class UserFileRepository : GenericRepository<UserFile, Guid>, IUserFileRepository
{
    private readonly DbConnection _dbConnection;

    public UserFileRepository(
        ApplicationDbContext context)
       : base(context)
    {
        _dbConnection = context.Database.GetDbConnection();
    }

    public async Task<IEnumerable<UserFile>> GetFilesByUserIdAsync(int userId)
    {
        const string sql = "SELECT * FROM \"UserFiles\" WHERE \"UserId\" = @UserId ORDER BY \"UploadedAt\" DESC";

        return await _dbConnection.QueryAsync<UserFile>(
            sql,
            new { UserId = userId }
        );
    }
    
    public override async Task<UserFile?> GetByIdAsync(Guid id)
    {
        const string sql = "SELECT * FROM \"UserFiles\" WHERE \"Id\" = @Id";

        return await _dbConnection.QueryFirstOrDefaultAsync<UserFile>(
            sql,
            new { Id = id }
        );
    }
}