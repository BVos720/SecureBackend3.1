using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class KindController : ControllerBase
{
    private readonly IKind _iKind;
    private readonly IOuder _iOuder;

    public KindController(IKind kindRepository, IOuder ouderRepository)
    {
        _iKind = kindRepository;
        _iOuder = ouderRepository;
    }

    [HttpGet(Name = "GetKinderen")]
    public async Task<ActionResult<List<Kind>>> GetAsync()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectByAccountIdAsync(userIdClaim);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = "Geen ouder gevonden voor de ingelogde gebruiker." });

        var kinderen = await _iKind.SelectAsync();
        return Ok(kinderen.Where(k => k.OuderID == ouder.OuderID).ToList());
    }

    [HttpGet("{kindID}", Name = "GetKindById")]
    public async Task<ActionResult<Kind>> GetByIdAsync(Guid kindID)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectByAccountIdAsync(userIdClaim);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = "Geen ouder gevonden voor de ingelogde gebruiker." });

        var kind = await _iKind.SelectAsync(kindID);

        if (kind == null)
            return NotFound(new ProblemDetails { Detail = $"Kind {kindID} not found" });

        if (kind.OuderID != ouder.OuderID)
            return Forbid();

        return Ok(kind);
    }

    [HttpPost(Name = "AddKind")]
    public async Task<ActionResult<Kind>> AddAsync(Kind kind)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectByAccountIdAsync(userIdClaim);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = "Geen ouder gevonden voor de ingelogde gebruiker." });

        kind.KindID = Guid.NewGuid();
        kind.OuderID = ouder.OuderID;

        await _iKind.InsertAsync(kind);

        return CreatedAtRoute("GetKindById", new { kindID = kind.KindID }, kind);
    }

    [HttpPut("{kindID}", Name = "UpdateKind")]
    public async Task<ActionResult<Kind>> UpdateAsync(Guid kindID, Kind kind)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectByAccountIdAsync(userIdClaim);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = "Geen ouder gevonden voor de ingelogde gebruiker." });

        var existing = await _iKind.SelectAsync(kindID);
        if (existing == null)
            return NotFound(new ProblemDetails { Detail = $"Kind {kindID} not found" });

        if (existing.OuderID != ouder.OuderID)
            return Forbid();

        kind.KindID = kindID;
        kind.OuderID = existing.OuderID;

        await _iKind.UpdateAsync(kind);

        return Ok(kind);
    }

    [HttpDelete("{kindID}", Name = "DeleteKind")]
    public async Task<ActionResult> DeleteAsync(Guid kindID)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectByAccountIdAsync(userIdClaim);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = "Geen ouder gevonden voor de ingelogde gebruiker." });

        var kind = await _iKind.SelectAsync(kindID);

        if (kind == null)
            return NotFound(new ProblemDetails { Detail = $"Kind {kindID} not found" });

        if (kind.OuderID != ouder.OuderID)
            return Forbid();

        await _iKind.DeleteAsync(kindID);

        return Ok();
    }
}
