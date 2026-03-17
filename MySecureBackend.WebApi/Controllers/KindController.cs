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

    public KindController(IKind kindRepository)
    {
        _iKind = kindRepository;
    }

    [HttpGet(Name = "GetKinderen")]
    public async Task<ActionResult<List<Kind>>> GetAsync()
    {
        var kinderen = await _iKind.SelectAsync();
        return Ok(kinderen);
    }

    [HttpGet("{kindID}", Name = "GetKindById")]
    public async Task<ActionResult<Kind>> GetByIdAsync(Guid kindID)
    {
        var kind = await _iKind.SelectAsync(kindID);

        if (kind == null)
            return NotFound(new ProblemDetails { Detail = $"Kind {kindID} not found" });

        return Ok(kind);
    }

    [HttpPost(Name = "AddKind")]
    public async Task<ActionResult<Kind>> AddAsync(Kind kind)
    {
        kind.KindID = Guid.NewGuid();

        await _iKind.InsertAsync(kind);

        return CreatedAtRoute("GetKindById", new { kindID = kind.KindID }, kind);
    }

    [HttpPut("{kindID}", Name = "UpdateKind")]
    public async Task<ActionResult<Kind>> UpdateAsync(Guid kindID, Kind kind)
    {
        var existing = await _iKind.SelectAsync(kindID);
        if (existing == null)
            return NotFound(new ProblemDetails { Detail = $"Kind {kindID} not found" });

        if (kind.KindID != kindID)
            return Conflict(new ProblemDetails { Detail = "The id of the Kind in the route does not match the id of the Kind in the body" });

        await _iKind.UpdateAsync(kind);

        return Ok(kind);
    }

    [HttpDelete("{kindID}", Name = "DeleteKind")]
    public async Task<ActionResult> DeleteAsync(Guid kindID)
    {
        var kind = await _iKind.SelectAsync(kindID);

        if (kind == null)
            return NotFound(new ProblemDetails { Detail = $"Kind {kindID} not found" });

        await _iKind.deleteAsync(kindID);

        return Ok();
    }
}
