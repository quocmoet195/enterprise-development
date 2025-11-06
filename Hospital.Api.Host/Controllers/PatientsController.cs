using Hospital.Application.Contracts.Patients;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(IPatientService service, ILogger<PatientsController> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PatientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<PatientDto>> GetAll()
    {
        try
        {
            return Ok(service.GetAll());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all patients");
            return Problem(title: "Unable to fetch patients.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<PatientDto> Get(int id)
    {
        try
        {
            var dto = service.Get(id);
            return dto is null ? NotFound() : Ok(dto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting patient by id {Id}", id);
            return Problem(title: "Unable to fetch patient.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<PatientDto> Create([FromBody] PatientCreateUpdateDto dto)
    {
        if (dto is null) return BadRequest("Body is required.");

        try
        {
            var created = service.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation error creating patient");
            return BadRequest("Invalid data.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating patient");
            return Problem(title: "Unable to create patient.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Update(int id, [FromBody] PatientCreateUpdateDto dto)
    {
        if (id <= 0) return BadRequest("Invalid id.");
        if (dto is null) return BadRequest("Body is required.");

        try
        {
            var ok = service.Update(id, dto);
            return ok ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation error updating patient {Id}", id);
            return BadRequest("Invalid data.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating patient {Id}", id);
            return Problem(title: "Unable to update patient.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Delete(int id)
    {
        if (id <= 0) return BadRequest("Invalid id.");

        try
        {
            var ok = service.Delete(id);
            return ok ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting patient {Id}", id);
            return Problem(title: "Unable to delete patient.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
