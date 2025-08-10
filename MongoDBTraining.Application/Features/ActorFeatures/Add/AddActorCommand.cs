using MediatR;
using MongoDBTraining.Domain.Entities;
using MongoDBTraining.Domain.Interfaces.Repositories;

namespace MongoDBTraining.Application.Features.ActorFeatures.Add;

public record AddActorCommand(
    string Name,
    string LastName
) : IRequest<string>;

public class AddActorHandler : IRequestHandler<AddActorCommand, string>
{
    private readonly IActorRepository _repository;

    public AddActorHandler(IActorRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(AddActorCommand request, CancellationToken cst = default)
    {
        var actor = new Actor
        {
            Name = request.Name,
            LastName = request.LastName
        };

        await _repository.AddAsync(actor, cst);
        return actor.Id;
    }
}
