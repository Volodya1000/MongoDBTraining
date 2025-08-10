using MediatR;
using MongoDBTraining.Domain.Interfaces.Repositories;

namespace MongoDBTraining.Application.Features.ActorFeatures.Delete;

public record DeleteActorCommand(string Id) : IRequest<Unit>;

public class DeleteActorHandler : IRequestHandler<DeleteActorCommand, Unit>
{
    private readonly IActorRepository _repository;

    public DeleteActorHandler(IActorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteActorCommand request, CancellationToken cst = default)
    {
        var actor = await _repository.GetByIdAsync(request.Id, cst);
        if (actor == null)
            throw new KeyNotFoundException($"Actor with Id {request.Id} not found.");

        await _repository.DeleteAsync(actor, cst);
        return Unit.Value;
    }
}
