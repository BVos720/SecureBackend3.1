using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class BehandelingController : ControllerBase
{
    private readonly IBehandeling _iBehandeling;
    private readonly IOuder _iOuder;
    private readonly IKind _iKind;

    public BehandelingController(IBehandeling behandelingRepository, IOuder ouderRepository, IKind kindRepository)
    {
        _iBehandeling = behandelingRepository;
        _iOuder = ouderRepository;
        _iKind = kindRepository;
    }

    [HttpGet(Name = "GetBehandelingen")]
    public async Task<ActionResult<List<Behandeling>>> GetAsync()
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

        var behandelingen = await _iBehandeling.SelectAsync();
        return Ok(behandelingen.Where(b => b.KindID == kind.KindID).ToList());
    }

    [HttpGet("{behandelingID}", Name = "GetBehandelingById")]
    public async Task<ActionResult<Behandeling>> GetByIdAsync(Guid behandelingID)
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

        var behandeling = await _iBehandeling.SelectAsync(behandelingID);

        if (behandeling == null)
            return NotFound(new ProblemDetails { Detail = $"Behandeling {behandelingID} not found" });

        if (behandeling.KindID != kind.KindID)
            return Forbid();

        return Ok(behandeling);
    }

    [HttpPost(Name = "AddBehandeling")]
    public async Task<ActionResult<Behandeling>> AddAsync(Behandeling behandeling)
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

        behandeling.BehandelingID = Guid.NewGuid();
        behandeling.KindID = kind.KindID;

        await _iBehandeling.InsertAsync(behandeling);

        return CreatedAtRoute("GetBehandelingById", new { behandelingID = behandeling.BehandelingID }, behandeling);
    }

    [HttpPut("{behandelingID}", Name = "UpdateBehandeling")]
    public async Task<ActionResult<Behandeling>> UpdateAsync(Guid behandelingID, Behandeling behandeling)
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

        var existing = await _iBehandeling.SelectAsync(behandelingID);
        if (existing == null)
            return NotFound(new ProblemDetails { Detail = $"Behandeling {behandelingID} not found" });

        if (existing.KindID != kind.KindID)
            return Forbid();

        behandeling.BehandelingID = behandelingID;
        behandeling.KindID = existing.KindID;

        await _iBehandeling.UpdateAsync(behandeling);

        return Ok(behandeling);
    }

    [HttpDelete("{behandelingID}", Name = "DeleteBehandeling")]
    public async Task<ActionResult> DeleteAsync(Guid behandelingID)
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

        var behandeling = await _iBehandeling.SelectAsync(behandelingID);

        if (behandeling == null)
            return NotFound(new ProblemDetails { Detail = $"Behandeling {behandelingID} not found" });

        if (behandeling.KindID != kind.KindID)
            return Forbid();

        await _iBehandeling.deleteAsync(behandelingID);

        return Ok();
    }
}
