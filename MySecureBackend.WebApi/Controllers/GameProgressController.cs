using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.WebApi.Controllers;
//Test
[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class GameProgressController : ControllerBase
{
    private readonly IGameProgress _iGameProgress;
    private readonly IOuder _iOuder;
    private readonly IKind _iKind;
    private readonly IBehandeling _iBehandeling;

    public GameProgressController(IGameProgress gameProgressRepository, IOuder ouderRepository, IKind kindRepository, IBehandeling behandelingRepository)
    {
        _iGameProgress = gameProgressRepository;
        _iOuder = ouderRepository;
        _iKind = kindRepository;
        _iBehandeling = behandelingRepository;
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
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectByAccountIdAsync(userIdClaim);
        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = "Geen ouder gevonden voor de ingelogde gebruiker." });

        var kind = await _iKind.SelectByOuderIdAsync(ouder.OuderID);
        if (kind == null)
            return NotFound(new ProblemDetails { Detail = "Geen kind gevonden voor de ingelogde gebruiker." });

        var behandeling = await _iBehandeling.SelectByKindIdAsync(kind.KindID);
        if (behandeling == null)
            return NotFound(new ProblemDetails { Detail = "Geen behandeling gevonden voor de ingelogde gebruiker." });

        gameProgress.GameProgressID = Guid.NewGuid();
        gameProgress.BehandelingID = behandeling.BehandelingID;

        await _iGameProgress.InsertAsync(gameProgress);

        return CreatedAtRoute("GetGameProgressById", new { gameProgressID = gameProgress.GameProgressID }, gameProgress);
    }

    [HttpPut("{gameProgressID}", Name = "UpdateGameProgress")]
    public async Task<ActionResult<GameProgress>> UpdateAsync(Guid gameProgressID, GameProgress gameProgress)
    {
        var existing = await _iGameProgress.SelectAsync(gameProgressID);
        if (existing == null)
            return NotFound(new ProblemDetails { Detail = $"GameProgress {gameProgressID} not found" });

        gameProgress.GameProgressID = gameProgressID;
        gameProgress.BehandelingID = existing.BehandelingID;

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
