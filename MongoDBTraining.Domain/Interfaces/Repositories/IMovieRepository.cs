using MongoDBTraining.Domain.Entities;

namespace MongoDBTraining.Domain.Interfaces.Repositories;

public interface IMovieRepository
{
    Task AddAsync(Movie movie, CancellationToken cst = default);
    Task<IReadOnlyList<Movie>> GetAllAsync(CancellationToken cst = default);
}
