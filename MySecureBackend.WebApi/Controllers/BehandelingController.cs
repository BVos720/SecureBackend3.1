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

    public BehandelingController(IBehandeling behandelingRepository)
    {
        _iBehandeling = behandelingRepository;
    }

    [HttpGet(Name = "GetBehandelingen")]
    public async Task<ActionResult<List<Behandeling>>> GetAsync()
    {
        var behandelingen = await _iBehandeling.SelectAsync();
        return Ok(behandelingen);
    }

    [HttpGet("{behandelingID}", Name = "GetBehandelingById")]
    public async Task<ActionResult<Behandeling>> GetByIdAsync(Guid behandelingID)
    {
        var behandeling = await _iBehandeling.SelectAsync(behandelingID);

        if (behandeling == null)
            return NotFound(new ProblemDetails { Detail = $"Behandeling {behandelingID} not found" });

        return Ok(behandeling);
    }

    [HttpPost(Name = "AddBehandeling")]
    public async Task<ActionResult<Behandeling>> AddAsync(Behandeling behandeling)
    {
        behandeling.BehandelingID = Guid.NewGuid();

        await _iBehandeling.InsertAsync(behandeling);

        return CreatedAtRoute("GetBehandelingById", new { behandelingID = behandeling.BehandelingID }, behandeling);
    }

    [HttpPut("{behandelingID}", Name = "UpdateBehandeling")]
    public async Task<ActionResult<Behandeling>> UpdateAsync(Guid behandelingID, Behandeling behandeling)
    {
        var existing = await _iBehandeling.SelectAsync(behandelingID);
        if (existing == null)
            return NotFound(new ProblemDetails { Detail = $"Behandeling {behandelingID} not found" });

        if (behandeling.BehandelingID != behandelingID)
            return Conflict(new ProblemDetails { Detail = "The id of the Behandeling in the route does not match the id of the Behandeling in the body" });

        await _iBehandeling.UpdateAsync(behandeling);

        return Ok(behandeling);
    }

    [HttpDelete("{behandelingID}", Name = "DeleteBehandeling")]
    public async Task<ActionResult> DeleteAsync(Guid behandelingID)
    {
        var behandeling = await _iBehandeling.SelectAsync(behandelingID);

        if (behandeling == null)
            return NotFound(new ProblemDetails { Detail = $"Behandeling {behandelingID} not found" });

        await _iBehandeling.deleteAsync(behandelingID);

        return Ok();
    }
}
