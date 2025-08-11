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

    /// <summary>
    /// Добавляет актёра к фильму и фильм к актёру в одной транзакции.
    /// </summary>
    public async Task AddActorToMovieAsync(Movie movie, Actor actor, CancellationToken cst = default)
    {
        using var session = await _context.Client.StartSessionAsync(cancellationToken: cst);
        session.StartTransaction();

        try
        {
            var updateMovie = Builders<Movie>.Update.AddToSet(m => m.ActorIds, actor.Id);
            await _context.Movies.UpdateOneAsync(session, m => m.Id == movie.Id, updateMovie, cancellationToken: cst);

            var updateActor = Builders<Actor>.Update.AddToSet(a => a.MovieIds, movie.Id);
            await _context.Actors.UpdateOneAsync(session, a => a.Id == actor.Id, updateActor, cancellationToken: cst);

            await session.CommitTransactionAsync(cancellationToken: cst);
        }
        catch
        {
            await session.AbortTransactionAsync(cancellationToken: cst);
            throw;
        }
    }

    /// <summary>
    /// Удаляет фильм и удаляет его id из всех актёров, у которых он есть.
    /// </summary>
    public override async Task DeleteAsync(Movie movie, CancellationToken cst = default)
    {
        using var session = await _context.Client.StartSessionAsync(cancellationToken: cst);
        session.StartTransaction();

        try
        {
            // Удалить фильм
            await _context.Movies.DeleteOneAsync(session, m => m.Id == movie.Id, cancellationToken: cst);

            // Удалить movieId у всех актёров
            var updateActors = Builders<Actor>.Update.Pull(a => a.MovieIds, movie.Id);
            await _context.Actors.UpdateManyAsync(session, a => a.MovieIds.Contains(movie.Id), updateActors, cancellationToken: cst);

            await session.CommitTransactionAsync(cancellationToken: cst);
        }
        catch
        {
            await session.AbortTransactionAsync(cancellationToken: cst);
            throw;
        }
    }

}
