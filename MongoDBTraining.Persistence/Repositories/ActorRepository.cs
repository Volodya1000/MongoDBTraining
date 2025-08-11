using MongoDB.Driver;
using MongoDBTraining.Domain.Entities;
using MongoDBTraining.Domain.Interfaces.Repositories;

namespace MongoDBTraining.Persistence.Repositories;

public class ActorRepository:BaseRepository<Actor>,IActorRepository
{
    private readonly ApplicationDbContext _context;

    public ActorRepository(ApplicationDbContext context)
        : base(context.Actors, context.Client)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public override async Task DeleteAsync(Actor actor, CancellationToken cst = default)
    {
        using var session = await _context.Client.StartSessionAsync(cancellationToken: cst);
        session.StartTransaction();

        try
        {
            // Удаляем актёра
            await _context.Actors.DeleteOneAsync(session, a => a.Id == actor.Id, cancellationToken: cst);

            // Удаляем actorId из всех movies
            var update = Builders<Movie>.Update.Pull(m => m.ActorIds, actor.Id);
            await _context.Movies.UpdateManyAsync(session, m => m.ActorIds.Contains(actor.Id), update, cancellationToken: cst);

            await session.CommitTransactionAsync(cancellationToken: cst);
        }
        catch
        {
            await session.AbortTransactionAsync(cancellationToken: cst);
            throw;
        }
    }
}
