using AzureBlobManager.Application.Common.Dependencies.DataAccess;
using AzureBlobManager.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AzureBlobManager.Infrastructure.Common.Dependencies.DataAccess;

internal abstract class GenericRepository<T, TKey> : IRepository<T, TKey> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public virtual async Task<T?> GetByIdAsync(TKey tKey)
    {
        return await _dbSet.FindAsync(tKey);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual Task UpdateAsync(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }
}