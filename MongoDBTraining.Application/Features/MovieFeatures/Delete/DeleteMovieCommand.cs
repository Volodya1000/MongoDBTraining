using MediatR;
using MongoDBTraining.Domain.Interfaces.Repositories;

namespace MongoDBTraining.Application.Features.MovieFeatures.Delete;

public record DeleteMovieCommand(string Id) : IRequest<Unit>;

public class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand,Unit>
{
    private readonly IMovieRepository _repository;

    public DeleteMovieHandler(IMovieRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteMovieCommand request, CancellationToken cst = default)
    {
        var movie = await _repository.GetByIdAsync(request.Id, cst);
        if (movie == null)
            throw new KeyNotFoundException($"Movie with Id {request.Id} not found.");

        await _repository.DeleteAsync(movie, cst);
        return Unit.Value;
    }
}

