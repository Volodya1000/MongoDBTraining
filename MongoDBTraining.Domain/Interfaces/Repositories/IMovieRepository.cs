using MongoDBTraining.Domain.Entities;

namespace MongoDBTraining.Domain.Interfaces.Repositories;

public interface IMovieRepository : IRepository<Movie>
{
    Task AddActorToMovieAsync(string movieId, string actorId, CancellationToken cst = default);
}
