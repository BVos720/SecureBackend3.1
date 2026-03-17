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
    public async Task<ActionResult<List<Ouder>>> GetAsync()
    {
        var ouders = await _iOuder.SelectAsync();
        return Ok(ouders);
    }

    [HttpGet("{ouderID}", Name = "GetOuderById")]
    public async Task<ActionResult<Ouder>> GetByIdAsync(Guid ouderID)
    {
        var ouder = await _iOuder.SelectAsync(ouderID);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = $"Ouder {ouderID} not found" });

        return Ok(ouder);
    }

    [HttpPost(Name = "AddOuder")]
    public async Task<ActionResult<Ouder>> AddAsync(Ouder ouder)
    {
        ouder.OuderID = Guid.NewGuid();

        await _iOuder.InsertAsync(ouder);

        return CreatedAtRoute("GetOuderById", new { ouderID = ouder.OuderID }, ouder);
    }

    [HttpPut("{ouderID}", Name = "UpdateOuder")]
    public async Task<ActionResult<Ouder>> UpdateAsync(Guid ouderID, Ouder ouder)
    {
        var existing = await _iOuder.SelectAsync(ouderID);
        if (existing == null)
            return NotFound(new ProblemDetails { Detail = $"Ouder {ouderID} not found" });

        if (ouder.OuderID != ouderID)
            return Conflict(new ProblemDetails { Detail = "The id of the Ouder in the route does not match the id of the Ouder in the body" });

        await _iOuder.UpdateAsync(ouder);

        return Ok(ouder);
    }

    [HttpDelete("{ouderID}", Name = "DeleteOuder")]
    public async Task<ActionResult> DeleteAsync(Guid ouderID)
    {
        var ouder = await _iOuder.SelectAsync(ouderID);

        if (ouder == null)
            return NotFound(new ProblemDetails { Detail = $"Ouder {ouderID} not found" });

        await _iOuder.deleteAsync(ouderID);

        return Ok();
    }
}
