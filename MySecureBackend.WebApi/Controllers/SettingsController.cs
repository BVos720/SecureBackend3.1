using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;

namespace MySecureBackend.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class SettingsController : ControllerBase
{
    private readonly ISettings _iSettings;
    private readonly IKind _iKind;
    private readonly IOuder _iOuder;

    public SettingsController(ISettings settingsRepository, IKind kindRepository, IOuder ouderRepository)
    {
        _iSettings = settingsRepository;
        _iKind = kindRepository;
        _iOuder = ouderRepository;
    }

    [HttpGet(Name = "GetSettings")]
    public async Task<ActionResult<Settings>> GetAsync()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectByAccountIdAsync(userIdClaim);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = "Geen ouder gevonden voor de ingelogde gebruiker." });

        var kind = await _iKind.SelectByOuderIdAsync(ouder.OuderID);

        if (kind == null)
            return NotFound(new ProblemDetails { Detail = "Geen kind gevonden voor de ingelogde ouder." });

        var settings = await _iSettings.SelectByKindIdAsync(kind.KindID);

        if (settings == null)
            return NotFound(new ProblemDetails { Detail = "Geen settings gevonden voor dit kind." });

        return Ok(settings);
    }

    [HttpGet("{settingsID}", Name = "GetSettingsById")]
    public async Task<ActionResult<Settings>> GetByIdAsync(Guid settingsID)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectByAccountIdAsync(userIdClaim);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = "Geen ouder gevonden voor de ingelogde gebruiker." });

        var settings = await _iSettings.SelectAsync(settingsID);

        if (settings == null)
            return NotFound(new ProblemDetails { Detail = $"Settings {settingsID} niet gevonden." });

        var kind = await _iKind.SelectAsync(settings.KindID);

        if (kind == null || kind.OuderID != ouder.OuderID)
            return Forbid();

        return Ok(settings);
    }

    [HttpPost(Name = "AddSettings")]
    public async Task<ActionResult<Settings>> AddAsync(Settings settings)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectByAccountIdAsync(userIdClaim);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = "Geen ouder gevonden voor de ingelogde gebruiker." });

        var kind = await _iKind.SelectAsync(settings.KindID);

        if (kind == null)
            return NotFound(new ProblemDetails { Detail = $"Kind {settings.KindID} niet gevonden." });

        if (kind.OuderID != ouder.OuderID)
            return Forbid();

        settings.SettingsID = Guid.NewGuid();

        await _iSettings.InsertAsync(settings);

        return CreatedAtRoute("GetSettingsById", new { settingsID = settings.SettingsID }, settings);
    }

    [HttpPut("{settingsID}", Name = "UpdateSettings")]
    public async Task<ActionResult<Settings>> UpdateAsync(Guid settingsID, Settings settings)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectByAccountIdAsync(userIdClaim);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = "Geen ouder gevonden voor de ingelogde gebruiker." });

        var existing = await _iSettings.SelectAsync(settingsID);

        if (existing == null)
            return NotFound(new ProblemDetails { Detail = $"Settings {settingsID} niet gevonden." });

        var kind = await _iKind.SelectAsync(existing.KindID);

        if (kind == null || kind.OuderID != ouder.OuderID)
            return Forbid();

        settings.SettingsID = settingsID;
        settings.KindID = existing.KindID;

        await _iSettings.UpdateAsync(settings);

        return Ok(settings);
    }

    [HttpDelete("{settingsID}", Name = "DeleteSettings")]
    public async Task<ActionResult> DeleteAsync(Guid settingsID)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectByAccountIdAsync(userIdClaim);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = "Geen ouder gevonden voor de ingelogde gebruiker." });

        var settings = await _iSettings.SelectAsync(settingsID);

        if (settings == null)
            return NotFound(new ProblemDetails { Detail = $"Settings {settingsID} niet gevonden." });

        var kind = await _iKind.SelectAsync(settings.KindID);

        if (kind == null || kind.OuderID != ouder.OuderID)
            return Forbid();

        await _iSettings.deleteAsync(settingsID);

        return Ok();
    }
}
