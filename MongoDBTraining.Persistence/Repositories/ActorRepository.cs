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
}
