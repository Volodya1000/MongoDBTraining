using MediatR;
using MongoDBTraining.Domain.Interfaces.Repositories;

namespace MongoDBTraining.Application.Features.ActorFeatures.Update;

public record UpdateActorCommand(
    string Id,
    string Name,
    string LastName
) : IRequest<Unit>;

public class UpdateActorHandler : IRequestHandler<UpdateActorCommand, Unit>
{
    private readonly IActorRepository _repository;

    public UpdateActorHandler(IActorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateActorCommand request, CancellationToken cst = default)
    {
        var existing = await _repository.GetByIdAsync(request.Id, cst);
        if (existing == null)
            throw new KeyNotFoundException($"Actor with Id {request.Id} not found.");

        existing.Name = request.Name;
        existing.LastName = request.LastName;

        await _repository.UpdateAsync(existing, cst);
        return Unit.Value;
    }
}
