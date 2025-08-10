using MediatR;
using MongoDBTraining.Domain.Entities;
using MongoDBTraining.Domain.Interfaces.Repositories;

namespace MongoDBTraining.Application.Features.ActorFeatures.GetAll;

public record GetAllActorsQuery() : IRequest<IReadOnlyList<Actor>>;

public class GetAllActorsHandler : IRequestHandler<GetAllActorsQuery, IReadOnlyList<Actor>>
{
    private readonly IActorRepository _repository;

    public GetAllActorsHandler(IActorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<Actor>> Handle(GetAllActorsQuery request, CancellationToken cst = default)
    {
        return await _repository.GetAllAsync(cst);
    }
}