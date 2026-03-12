using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class Enviroment2DControler : ControllerBase
{
    private readonly IEnviroment2D _ienviroment2d;
    private readonly IObject2D _iobject2d;
    private readonly IAuthenticationService _authenticationService;

    public Enviroment2DControler(IEnviroment2D enviromentRepository, IObject2D objectRepository, IAuthenticationService authenticationService)
    {
        _ienviroment2d = enviromentRepository;
        _iobject2d = objectRepository;
        _authenticationService = authenticationService;
    }

    [HttpGet(Name = "Get2DEnviroment")]
    public async Task<ActionResult<List<Enviroment2D>>> GetAsync()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var enviroments2D = await _ienviroment2d.SelectByUserAsync(userIdClaim);
        return Ok(enviroments2D);
    }

    [HttpGet("{enviroment2dID}", Name = "GetEnviroment2D")]
    public async Task<ActionResult<Enviroment2D>> GetByIdAsync(Guid enviroment2dID)
    {
        var enviroment2D = await _ienviroment2d.SelectAsync(enviroment2dID);

        if (enviroment2D == null)
            return NotFound(new ProblemDetails { Detail = $"Environment2D {enviroment2dID} not found." });

        return Ok(enviroment2D);
    }

    [HttpPost(Name = "AddEnviroment2D")]
    public async Task<ActionResult<Enviroment2D>> AddAsync(Enviroment2D enviroment2d)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var bestaandeWerelden = await _ienviroment2d.SelectByUserAsync(userIdClaim);
        if (bestaandeWerelden.Count() >= 5)
            return BadRequest(new ProblemDetails { Detail = "Je mag niet meer dan 5 werelden aanmaken." });

        if (bestaandeWerelden.Any(e => e.Name == enviroment2d.Name))
            return BadRequest(new ProblemDetails { Detail = "Een wereld met deze naam bestaat al." });

        enviroment2d.Id = Guid.NewGuid();
        enviroment2d.UserID = userIdClaim;

        await _ienviroment2d.InsertAsync(enviroment2d);

        return CreatedAtRoute("GetEnviroment2D", new { enviroment2dID = enviroment2d.Id }, enviroment2d);
    }

    [HttpPut("{enviroment2dID}", Name = "UpdateEnviroment2D")]
    public async Task<ActionResult<Enviroment2D>> UpdateAsync(Guid enviroment2dID, Enviroment2D enviroment2D)
    {
        var existingEnviroment = await _ienviroment2d.SelectAsync(enviroment2dID);

        if (existingEnviroment == null)
            return NotFound(new ProblemDetails { Detail = $"Environment2D {enviroment2dID} not found." });

        if (enviroment2D.Id != enviroment2dID)
            return Conflict(new ProblemDetails { Detail = "The ID of the Environment2D in the route does not match the ID in the body." });

        await _ienviroment2d.UpdateAsync(enviroment2D);

        return Ok(enviroment2D);
    }

    [HttpDelete("{enviroment2dID}", Name = "DeleteEnviroment2D")]
    public async Task<ActionResult> DeleteAsync(Guid enviroment2dID)
    {
        var existingEnviroment = await _ienviroment2d.SelectAsync(enviroment2dID);

        if (existingEnviroment == null)
            return NotFound(new ProblemDetails { Detail = $"Environment2D {enviroment2dID} not found." });

        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if(existingEnviroment.UserID != userIdClaim)
        {
            return Forbid();
        }

        await _iobject2d.DeleteByEnvironmentAsync(enviroment2dID);
        await _ienviroment2d.deleteAsync(enviroment2dID);

        return Ok();
    }
}