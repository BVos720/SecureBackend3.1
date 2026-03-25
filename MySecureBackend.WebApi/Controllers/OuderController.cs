using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class OuderController : ControllerBase
{
    private readonly IOuder _iOuder;

    public OuderController(IOuder ouderRepository)
    {
        _iOuder = ouderRepository;
    }

    [HttpGet(Name = "GetOuders")]
    public async Task<ActionResult<Ouder>> GetAsync()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectByAccountIdAsync(userIdClaim);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = "Geen ouder gevonden voor de ingelogde gebruiker." });

        return Ok(ouder);
    }

    [HttpGet("{ouderID}", Name = "GetOuderById")]
    public async Task<ActionResult<Ouder>> GetByIdAsync(Guid ouderID)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectAsync(ouderID);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = $"Ouder {ouderID} not found" });

        if (ouder.AccountID != userIdClaim)
            return Forbid();

        return Ok(ouder);
    }

    [HttpPost(Name = "AddOuder")]
    public async Task<ActionResult<Ouder>> AddAsync(Ouder ouder)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        ouder.OuderID = Guid.NewGuid();
        ouder.AccountID = userIdClaim;

        await _iOuder.InsertAsync(ouder);

        return CreatedAtRoute("GetOuderById", new { ouderID = ouder.OuderID }, ouder);
    }

    [HttpPut("{ouderID}", Name = "UpdateOuder")]
    public async Task<ActionResult<Ouder>> UpdateAsync(Guid ouderID, Ouder ouder)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var existing = await _iOuder.SelectAsync(ouderID);
        if (existing == null)
            return NotFound(new ProblemDetails { Detail = $"Ouder {ouderID} not found" });

        if (existing.AccountID != userIdClaim)
            return Forbid();

        ouder.OuderID = ouderID;
        ouder.AccountID = existing.AccountID;

        await _iOuder.UpdateAsync(ouder);

        return Ok(ouder);
    }

    [HttpDelete("{ouderID}", Name = "DeleteOuder")]
    public async Task<ActionResult> DeleteAsync(Guid ouderID)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var ouder = await _iOuder.SelectAsync(ouderID);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = $"Ouder {ouderID} not found" });

        if (ouder.AccountID != userIdClaim)
            return Forbid();

        await _iOuder.DeleteAsync(ouderID);

        return Ok();
    }
}
