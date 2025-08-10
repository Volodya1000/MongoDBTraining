using MongoDBTraining.Domain.Common;

namespace MongoDBTraining.Domain.Interfaces.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity, CancellationToken cst = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cst = default);
    Task<T?> GetByIdAsync(string id, CancellationToken cst = default);
    Task UpdateAsync(T entity, CancellationToken cst = default);
    Task DeleteAsync(T entity, CancellationToken cst = default);
}