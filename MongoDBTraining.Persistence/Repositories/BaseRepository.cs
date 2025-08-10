using MongoDB.Driver;
using MongoDBTraining.Domain.Common;
using MongoDBTraining.Domain.Interfaces.Repositories;

namespace MongoDBTraining.Persistence.Repositories;

public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly IMongoCollection<T> Collection;
    protected readonly IMongoClient Client;

    public BaseRepository(IMongoCollection<T> collection, IMongoClient client)
    {
        Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        Client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public virtual Task AddAsync(T entity, CancellationToken cst = default) =>
        Collection.InsertOneAsync(entity, cancellationToken: cst);

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cst = default)
    {
        var items = await Collection.Find(_ => true).ToListAsync(cst);
        return items.AsReadOnly();
    }

    public virtual async Task<T?> GetByIdAsync(string id, CancellationToken cst = default)
    {
        return await Collection.Find(x => x.Id == id).FirstOrDefaultAsync(cst);
    }

    public virtual Task UpdateAsync(T entity, CancellationToken cst = default) =>
        Collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, cancellationToken: cst);

    public virtual Task DeleteAsync(T entity, CancellationToken cst = default) =>
        Collection.DeleteOneAsync(x => x.Id == entity.Id, cancellationToken: cst);
}

