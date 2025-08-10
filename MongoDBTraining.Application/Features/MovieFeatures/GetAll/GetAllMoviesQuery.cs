using MediatR;
using MongoDBTraining.Domain.Entities;
using MongoDBTraining.Domain.Interfaces.Repositories;

namespace MongoDBTraining.Application.Features.MovieFeatures.GetAll;
public class GetAllMoviesQuery : IRequest<IReadOnlyList<Movie>> { }

public class GetAllMoviesHandler : IRequestHandler<GetAllMoviesQuery, IReadOnlyList<Movie>>
{
    private readonly IMovieRepository _repository;

    public GetAllMoviesHandler(IMovieRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<Movie>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}