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
        return _context.Movies.InsertOneAsync(movie, options: null, cst);
    }

    public async Task<IReadOnlyList<Movie>> GetAllAsync(CancellationToken cst = default)
    {
        var movies = await _context.Movies.Find(_ => true).ToListAsync(cst);
        return movies.AsReadOnly();
    }
}
