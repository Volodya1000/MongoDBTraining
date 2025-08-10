using MediatR;
using MongoDBTraining.Domain.Interfaces.Repositories;

namespace MongoDBTraining.Application.Features.MovieFeatures.Update;

public record UpdateMovieCommand(
    string Id,
    string Name,
    string Description
) : IRequest<Unit>;

public class UpdateMovieHandler : IRequestHandler<UpdateMovieCommand, Unit>
{
    private readonly IMovieRepository _repository;

    public UpdateMovieHandler(IMovieRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateMovieCommand request, CancellationToken cst = default)
    {
        var existingMovie = await _repository.GetById(request.Id, cst);
        if (existingMovie == null)
            throw new KeyNotFoundException($"Movie with Id {request.Id} not found.");

        existingMovie.Name = request.Name;
        existingMovie.Description = request.Description;

        await _repository.UpdateAsync(existingMovie, cst);
        return Unit.Value;
    }
}