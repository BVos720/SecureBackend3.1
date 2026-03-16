using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class PatientController : ControllerBase
{
    private readonly IObject2D _iobject2d;
    private readonly IAuthenticationService _authenticationService;

    public PatientController(IObject2D objectrepository, IAuthenticationService authenticationService)
    {
        _iobject2d = objectrepository;
        _authenticationService = authenticationService;
    }

    [HttpGet(Name = "GetObject2D")]
    public async Task<ActionResult<List<Patient>>> GetAsync()
    {
        var object2d = await _iobject2d.SelectAsync();
        return Ok(object2d);
    }
    [HttpGet("{object2DID}", Name = "GetAllObject2D")]
    public async Task<ActionResult<Patient>> GetByIdAsync(Guid object2DID)
    {
        var object2D = await _iobject2d.SelectAsync(object2DID);

        if (object2D == null)
            return NotFound(new ProblemDetails { Detail = $"ExampleObject {object2DID} not found" });

        return Ok(object2D);
    }

    [HttpGet("environment/{environmentId}", Name = "GetObject2DsByEnvironment")]
    public async Task<ActionResult<IEnumerable<Patient>>> GetByEnvironmentAsync(Guid environmentId)
    {
        var objects = await _iobject2d.SelectByEnvironmentAsync(environmentId);
        return Ok(objects);
    }

    [HttpPost(Name = "AddObject2DID")]
    public async Task<ActionResult<Patient>> AddAsync(Patient object2d)
    {
        object2d.GUID = Guid.NewGuid();

        await _iobject2d.InsertAsync(object2d);

        return CreatedAtRoute("GetObject2D", new { Object2DID = object2d.GUID }, object2d);
    }

    [HttpPut("{Object2DID}", Name = "UpdateObject2D")]
    public async Task<ActionResult<Patient>> UpdateAsync(Guid object2DID, Patient object2D)
    {
        var existingObject2D = await _iobject2d.SelectAsync(object2DID);
        if (existingObject2D == null)
            return NotFound(new ProblemDetails { Detail = $"ExampleObject {object2DID} not found" });

        if (object2D.GUID != object2DID)
            return Conflict(new ProblemDetails { Detail = "The id of the ExampleObject in the route does not match the id of the ExampleObject in the body" });

        await _iobject2d.UpdateAsync(object2D);

        return Ok(object2D);
    }

    [HttpDelete("{object2DID}", Name = "DeleteObject2D")]
    public async Task<ActionResult> DeleteAsync(Guid object2DID)
    {
        var object2D = await _iobject2d.SelectAsync(object2DID);

        if (object2D == null)
            return NotFound(new ProblemDetails { Detail = $"ExampleObject {object2DID} not found" });

        await _iobject2d.deleteAsync(object2DID);

        return Ok();
    }
}