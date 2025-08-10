using MediatR;
using MongoDBTraining.Domain.Entities;
using MongoDBTraining.Domain.Interfaces.Repositories;

namespace MongoDBTraining.Application.Features.MovieFeatures.Add;

public record AddMovieCommand(
    string Name,
    string Description
) : IRequest<string>;

public class AddMovieHandler : IRequestHandler<AddMovieCommand,string>
{
    private readonly IMovieRepository _repository;

    public AddMovieHandler(IMovieRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(AddMovieCommand request, CancellationToken cst=default)
    {
        var movie = new Movie
        {
            Name = request.Name,
            Description = request.Description
        };

        await _repository.AddAsync(movie, cst);
        return movie.Id;
    }
}

