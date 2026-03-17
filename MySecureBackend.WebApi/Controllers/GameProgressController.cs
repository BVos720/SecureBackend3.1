using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class GameProgressController : ControllerBase
{
    private readonly IGameProgress _iGameProgress;

    public GameProgressController(IGameProgress gameProgressRepository)
    {
        _iGameProgress = gameProgressRepository;
    }

    [HttpGet(Name = "GetGameProgresses")]
    public async Task<ActionResult<List<GameProgress>>> GetAsync()
    {
        var gameProgresses = await _iGameProgress.SelectAsync();
        return Ok(gameProgresses);
    }

    [HttpGet("{gameProgressID}", Name = "GetGameProgressById")]
    public async Task<ActionResult<GameProgress>> GetByIdAsync(Guid gameProgressID)
    {
        var gameProgress = await _iGameProgress.SelectAsync(gameProgressID);

        if (gameProgress == null)
            return NotFound(new ProblemDetails { Detail = $"GameProgress {gameProgressID} not found" });

        return Ok(gameProgress);
    }

    [HttpPost(Name = "AddGameProgress")]
    public async Task<ActionResult<GameProgress>> AddAsync(GameProgress gameProgress)
    {
        gameProgress.GameProgressID = Guid.NewGuid();

        await _iGameProgress.InsertAsync(gameProgress);

        return CreatedAtRoute("GetGameProgressById", new { gameProgressID = gameProgress.GameProgressID }, gameProgress);
    }

    [HttpPut("{gameProgressID}", Name = "UpdateGameProgress")]
    public async Task<ActionResult<GameProgress>> UpdateAsync(Guid gameProgressID, GameProgress gameProgress)
    {
        var existing = await _iGameProgress.SelectAsync(gameProgressID);
        if (existing == null)
            return NotFound(new ProblemDetails { Detail = $"GameProgress {gameProgressID} not found" });

        if (gameProgress.GameProgressID != gameProgressID)
            return Conflict(new ProblemDetails { Detail = "The id of the GameProgress in the route does not match the id of the GameProgress in the body" });

        await _iGameProgress.UpdateAsync(gameProgress);

        return Ok(gameProgress);
    }

    [HttpDelete("{gameProgressID}", Name = "DeleteGameProgress")]
    public async Task<ActionResult> DeleteAsync(Guid gameProgressID)
    {
        var gameProgress = await _iGameProgress.SelectAsync(gameProgressID);

        if (gameProgress == null)
            return NotFound(new ProblemDetails { Detail = $"GameProgress {gameProgressID} not found" });

        await _iGameProgress.deleteAsync(gameProgressID);

        return Ok();
    }
}
