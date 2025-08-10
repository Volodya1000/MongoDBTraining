using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDBTraining.Application.Features.ActorFeatures.Add;
using MongoDBTraining.Application.Features.ActorFeatures.Delete;
using MongoDBTraining.Application.Features.ActorFeatures.GetAll;
using MongoDBTraining.Application.Features.ActorFeatures.Update;
using MongoDBTraining.Domain.Entities;

namespace MongoDBTraining.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Actor>>> GetAll(CancellationToken cst = default)
    {
        var actors = await _mediator.Send(new GetAllActorsQuery(), cst);
        return Ok(actors);
    }

    [HttpPost]
    public async Task<ActionResult> Add([FromBody] AddActorCommand command, CancellationToken cst = default)
    {
        await _mediator.Send(command, cst);
        return CreatedAtAction(nameof(GetAll), null);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateActorCommand command, CancellationToken cst = default)
    {
        await _mediator.Send(command, cst);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromBody] DeleteActorCommand command, CancellationToken cst = default)
    {
        await _mediator.Send(command, cst);
        return NoContent();
    }
}

