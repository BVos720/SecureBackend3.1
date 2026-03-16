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
    private readonly IPatient _ipatient;
    private readonly IAuthenticationService _authenticationService;

    public PatientController(IPatient patientRepository, IAuthenticationService authenticationService)
    {
        _ipatient = patientRepository;
        _authenticationService = authenticationService;
    }

    [HttpGet(Name = "GetPatients")]
    public async Task<ActionResult<List<Patient>>> GetAsync()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        var patients = await _ipatient.SelectByUserAsync(userIdClaim);
        return Ok(patients);
    }

    [HttpGet("{patientID}", Name = "GetPatientById")]
    public async Task<ActionResult<Patient>> GetByIdAsync(Guid patientID)
    {
        var patient = await _ipatient.SelectAsync(patientID);

        if (patient == null)
            return NotFound(new ProblemDetails { Detail = $"Patient {patientID} not found" });

        return Ok(patient);
    }

    [HttpPost(Name = "AddPatient")]
    public async Task<ActionResult<Patient>> AddAsync(Patient patient)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Geen geldige gebruikerssessie gevonden.");

        patient.PatientID = Guid.NewGuid();
        patient.UserID = userIdClaim;

        await _ipatient.InsertAsync(patient);

        return CreatedAtRoute("GetPatientById", new { patientID = patient.PatientID }, patient);
    }

    [HttpPut("{patientID}", Name = "UpdatePatient")]
    public async Task<ActionResult<Patient>> UpdateAsync(Guid patientID, Patient patient)
    {
        var existingPatient = await _ipatient.SelectAsync(patientID);
        if (existingPatient == null)
            return NotFound(new ProblemDetails { Detail = $"Patient {patientID} not found" });

        if (patient.PatientID != patientID)
            return Conflict(new ProblemDetails { Detail = "The id of the Patient in the route does not match the id of the Patient in the body" });

        await _ipatient.UpdateAsync(patient);

        return Ok(patient);
    }

    [HttpDelete("{patientID}", Name = "DeletePatient")]
    public async Task<ActionResult> DeleteAsync(Guid patientID)
    {
        var patient = await _ipatient.SelectAsync(patientID);

        if (patient == null)
            return NotFound(new ProblemDetails { Detail = $"Patient {patientID} not found" });

        await _ipatient.deleteAsync(patientID);

        return Ok();
    }
}
