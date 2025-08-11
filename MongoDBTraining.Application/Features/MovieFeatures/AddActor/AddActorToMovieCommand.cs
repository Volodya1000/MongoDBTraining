using MediatR;
using MongoDBTraining.Domain.Interfaces.Repositories;
namespace MongoDBTraining.Application.Features.MovieFeatures.AddActor;

public record AddActorToMovieCommand(string MovieId, string ActorId) : IRequest<Unit>;

public class AddActorToMovieHandler : IRequestHandler<AddActorToMovieCommand, Unit>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IActorRepository _actorRepository;

    public AddActorToMovieHandler(IMovieRepository movieRepository, IActorRepository actorRepository)
    {
        _movieRepository = movieRepository;
        _actorRepository = actorRepository;
    }

    public async Task<Unit> Handle(AddActorToMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId, cancellationToken);
        if (movie == null)
            throw new KeyNotFoundException($"Movie with Id {request.MovieId} not found.");

        var actor = await _actorRepository.GetByIdAsync(request.ActorId, cancellationToken);
        if (actor == null)
            throw new KeyNotFoundException($"Actor with Id {request.ActorId} not found.");

        await _movieRepository.AddActorToMovieAsync(movie, actor, cancellationToken);
        return Unit.Value;
    }
}