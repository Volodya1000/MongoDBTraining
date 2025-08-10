using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDBTraining.Application.Features.MovieFeatures.Add;
using MongoDBTraining.Application.Features.MovieFeatures.Delete;
using MongoDBTraining.Application.Features.MovieFeatures.GetAll;
using MongoDBTraining.Application.Features.MovieFeatures.Update;
using MongoDBTraining.Domain.Entities;
namespace MongoDBTraining.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Movie>>> GetAll(CancellationToken cst = default)
    {
        var movies = await _mediator.Send(new GetAllMoviesQuery(), cst);
        return Ok(movies);
    }

    [HttpPost]
    public async Task<ActionResult> Add([FromBody] AddMovieCommand command, CancellationToken cst=default)
    {
        await _mediator.Send(command, cst);
        return CreatedAtAction(nameof(GetAll), null);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateMovieCommand command, CancellationToken cst = default)
    {
        await _mediator.Send(command, cst);
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromBody] DeleteMovieCommand command, CancellationToken cst = default)
    {
        await _mediator.Send(command, cst);
        return NoContent();
    }
}
