using MongoDB.Driver;
using MongoDBTraining.Domain.Entities;
using MongoDBTraining.Domain.Interfaces.Repositories;

namespace MongoDBTraining.Persistence.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Movie movie, CancellationToken cst = default)
    {
        return _context.Movies.InsertOneAsync(movie, cancellationToken: cst);
    }

    public async Task<IReadOnlyList<Movie>> GetAllAsync(CancellationToken cst = default)
    {
        var movies = await _context.Movies.Find(_ => true).ToListAsync(cst);
        return movies.AsReadOnly();
    }

    public Task UpdateAsync(Movie movie, CancellationToken cst = default)
    {
        return _context.Movies.ReplaceOneAsync(m => m.Id == movie.Id, movie, cancellationToken: cst);
    }

    public  Task DeleteAsync(Movie movie, CancellationToken cst = default)
    {
        return _context.Movies.DeleteOneAsync(m => m.Id == movie.Id, cancellationToken: cst);
    }

    public async Task<Movie?> GetById(string id, CancellationToken cst = default)
    {
        return await _context.Movies.Find(m => m.Id == id).FirstOrDefaultAsync(cst);
    }
}
