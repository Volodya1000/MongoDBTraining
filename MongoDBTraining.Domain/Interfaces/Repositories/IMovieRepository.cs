using MongoDBTraining.Domain.Entities;

namespace MongoDBTraining.Domain.Interfaces.Repositories;

public interface IMovieRepository
{
    Task AddAsync(Movie movie, CancellationToken cst = default);
    Task<IReadOnlyList<Movie>> GetAllAsync(CancellationToken cst = default);
    Task<Movie?> GetById(string id, CancellationToken cst = default);
    Task UpdateAsync(Movie movie, CancellationToken cst = default);
    Task DeleteAsync(Movie movie, CancellationToken cst = default);
}
