namespace AzureBlobManager.Application.Common.Dependencies.DataAccess;

public interface IRepository<T, TKey> where T : class
{
    Task<T?> GetByIdAsync(TKey tKey);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}