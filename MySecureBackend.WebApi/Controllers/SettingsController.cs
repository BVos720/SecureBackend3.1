using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

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
    private readonly IAuthenticationService _authService;

    public SettingsController(ISettings settingsRepository, IKind kindRepository, IOuder ouderRepository, IAuthenticationService authService)
    {
        _iSettings = settingsRepository;
        _iKind = kindRepository;
        _iOuder = ouderRepository;
        _authService = authService;
    }

    [HttpGet(Name = "GetSettings")]
    public async Task<ActionResult<Settings>> GetAsync()
    {
        var userIdClaim = _authService.GetCurrentAuthenticatedUserId();

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
        var userIdClaim = _authService.GetCurrentAuthenticatedUserId();

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
        var userIdClaim = _authService.GetCurrentAuthenticatedUserId();

        var ouder = await _iOuder.SelectByAccountIdAsync(userIdClaim);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = "Geen ouder gevonden voor de ingelogde gebruiker." });

        var kind = await _iKind.SelectByOuderIdAsync(ouder.OuderID);

        if (kind == null)
            return NotFound(new ProblemDetails { Detail = "Geen kind gevonden voor de ingelogde ouder." });

        settings.KindID = kind.KindID;
        settings.SettingsID = Guid.NewGuid();

        await _iSettings.InsertAsync(settings);

        return CreatedAtRoute("GetSettingsById", new { settingsID = settings.SettingsID }, settings);
    }

    [HttpPut("{settingsID}", Name = "UpdateSettings")]
    public async Task<ActionResult<Settings>> UpdateAsync(Guid settingsID, Settings settings)
    {
        var userIdClaim = _authService.GetCurrentAuthenticatedUserId();

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
        var userIdClaim = _authService.GetCurrentAuthenticatedUserId();

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
