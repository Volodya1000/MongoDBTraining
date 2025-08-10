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
    /// Проверяет, что оба документа существуют.
    /// </summary>
    public async Task AddActorToMovieAsync(string movieId, string actorId, CancellationToken cst = default)
    {
        // Открываем сессию
        using var session = await _context.Client.StartSessionAsync(cancellationToken: cst);

        session.StartTransaction();

        try
        {
            // Проверяем, что фильм существует (читаем в контексте сессии)
            var movie = await _context.Movies.Find(session, m => m.Id == movieId)
                                             .FirstOrDefaultAsync(cst);
            if (movie == null)
                throw new KeyNotFoundException($"Movie with id='{movieId}' not found.");

            // Проверяем, что актёр существует
            var actor = await _context.Actors.Find(session, a => a.Id == actorId)
                                             .FirstOrDefaultAsync(cst);
            if (actor == null)
                throw new KeyNotFoundException($"Actor with id='{actorId}' not found.");

            // Добавить actorId в Movie.ActorIds, если ещё нет
            var updateMovie = Builders<Movie>.Update.AddToSet(m => m.ActorIds, actorId);
            await _context.Movies.UpdateOneAsync(session, m => m.Id == movieId, updateMovie, cancellationToken: cst);

            // Добавить movieId в Actor.MovieIds, если ещё нет
            var updateActor = Builders<Actor>.Update.AddToSet(a => a.MovieIds, movieId);
            await _context.Actors.UpdateOneAsync(session, a => a.Id == actorId, updateActor, cancellationToken: cst);

            // Всё ок — коммитим
            await session.CommitTransactionAsync(cancellationToken: cst);
        }
        catch
        {
            // На случай ошибки — откатываем
            await session.AbortTransactionAsync(cancellationToken: cst);
            throw;
        }
    }
}
