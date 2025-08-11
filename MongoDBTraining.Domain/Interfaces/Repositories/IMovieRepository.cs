using MongoDBTraining.Domain.Entities;

namespace MongoDBTraining.Domain.Interfaces.Repositories;

public interface IMovieRepository : IRepository<Movie>
{
    Task AddActorToMovieAsync(Movie movie, Actor actor, CancellationToken cst = default);
}
