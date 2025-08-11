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

    public override Task DeleteAsync(Actor actor, CancellationToken cst = default) =>
         _context.ExecuteInTransactionAsync(async session =>
         {
             await _context.Actors.DeleteOneAsync(session, a => a.Id == actor.Id, cancellationToken: cst);

             var updateMovies = Builders<Movie>.Update.Pull(m => m.ActorIds, actor.Id);
             await _context.Movies.UpdateManyAsync(session, m => m.ActorIds.Contains(actor.Id), updateMovies, cancellationToken: cst);
         }, cst);
}
