using MongoDB.Driver;
using MongoDBTraining.Persistence.Settings;
using Microsoft.Extensions.Options;
using MongoDBTraining.Domain.Entities;

namespace MongoDBTraining.Persistence;

public class ApplicationDbContext
{
    private readonly IMongoDatabase _database;

    public IMongoClient Client { get; }

    public ApplicationDbContext(IMongoClient mongoClient,IOptions<MongoSettings> options)
    {
        var settings = options.Value;
        _database = mongoClient.GetDatabase(settings.DatabaseName);
        Client = mongoClient;
    }

    public IMongoCollection<Movie> Movies => _database.GetCollection<Movie>(CollectionNames.Movies);

    public IMongoCollection<Actor> Actors => _database.GetCollection<Actor>(CollectionNames.Actors);

    /// <summary>
    /// Универсальный метод для выполнения операций в транзакции.
    /// </summary>
    public async Task ExecuteInTransactionAsync(Func<IClientSessionHandle, Task> action, CancellationToken cst = default)
    {
        using var session = await Client.StartSessionAsync(cancellationToken: cst);
        session.StartTransaction();

        try
        {
            await action(session);
            await session.CommitTransactionAsync(cancellationToken: cst);
        }
        catch
        {
            await session.AbortTransactionAsync(cancellationToken: cst);
            throw;
        }
    }
}
