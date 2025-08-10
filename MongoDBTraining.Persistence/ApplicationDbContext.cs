using MongoDB.Driver;
using MongoDBTraining.Persistence.Settings;
using Microsoft.Extensions.Options;
using MongoDBTraining.Domain.Entities;

namespace MongoDBTraining.Persistence;

public class ApplicationDbContext
{
    private readonly IMongoDatabase _database;

    public ApplicationDbContext(IMongoClient mongoClient,IOptions<MongoSettings> options)
    {
        var settings = options.Value;
        _database = mongoClient.GetDatabase(settings.DatabaseName);
    }


    public IMongoCollection<Movie> Movies => _database.GetCollection<Movie>(CollectionNames.Users);


}
