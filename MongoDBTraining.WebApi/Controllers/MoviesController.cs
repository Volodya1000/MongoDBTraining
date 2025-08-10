using Microsoft.AspNetCore.Mvc;
using MongoDBTraining.Domain.Entities;
using MongoDBTraining.Domain.Interfaces.Repositories;

namespace MongoDBTraining.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovieRepository _repository;

    public MoviesController(IMovieRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Movie>>> GetAll(CancellationToken cancellationToken)
    {
        var movies = await _repository.GetAllAsync(cancellationToken);
        return Ok(movies);
    }

    [HttpPost]
    public async Task<ActionResult> Add([FromBody] Movie movie, CancellationToken cancellationToken)
    {
        if (movie == null)
            return BadRequest("Movie cannot be null");

        await _repository.AddAsync(movie, cancellationToken);
        return CreatedAtAction(nameof(GetAll), null);
    }
}
