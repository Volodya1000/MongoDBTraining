using MongoDB.Driver;
using MongoDBTraining.Domain.Entities;
using MongoDBTraining.Domain.Interfaces.Repositories;

namespace MongoDBTraining.Persistence.Repositories;

public class MovieRepository : BaseRepository<Movie>, IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context)
        : base(context.Movies, context.Client)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task AddActorToMovieAsync(Movie movie, Actor actor, CancellationToken cst = default) =>
        _context.ExecuteInTransactionAsync(async session =>
        {
            var updateMovie = Builders<Movie>.Update.AddToSet(m => m.ActorIds, actor.Id);
            await _context.Movies.UpdateOneAsync(session, m => m.Id == movie.Id, updateMovie, cancellationToken: cst);

            var updateActor = Builders<Actor>.Update.AddToSet(a => a.MovieIds, movie.Id);
            await _context.Actors.UpdateOneAsync(session, a => a.Id == actor.Id, updateActor, cancellationToken: cst);
        }, cst);

    public override Task DeleteAsync(Movie movie, CancellationToken cst = default) =>
        _context.ExecuteInTransactionAsync(async session =>
        {
            await _context.Movies.DeleteOneAsync(session, m => m.Id == movie.Id, cancellationToken: cst);

            var updateActors = Builders<Actor>.Update.Pull(a => a.MovieIds, movie.Id);
            await _context.Actors.UpdateManyAsync(session, a => a.MovieIds.Contains(movie.Id), updateActors, cancellationToken: cst);
        }, cst);
}
