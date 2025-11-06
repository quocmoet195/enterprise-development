using Hospital.Application.Contracts.Appointments;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(IAppointmentService service, ILogger<AppointmentsController> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<AppointmentDto>> GetAll()
    {
        try
        {
            return Ok(service.GetAll());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all appointments");
            return Problem(title: "Unable to fetch appointments.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<AppointmentDto> Get(int id)
    {
        try
        {
            var dto = service.Get(id);
            return dto is null ? NotFound() : Ok(dto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting appointment by id {Id}", id);
            return Problem(title: "Unable to fetch appointment.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<AppointmentDto> Create([FromBody] AppointmentCreateUpdateDto dto)
    {
        if (dto is null) return BadRequest("Body is required.");
        if (dto.DoctorId <= 0 || dto.PatientId <= 0) return BadRequest("Valid DoctorId and PatientId are required.");

        try
        {
            var created = service.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "FK not found when creating appointment");
            return BadRequest("Referenced doctor or patient not found.");
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation error creating appointment");
            return BadRequest("Invalid data.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating appointment");
            return Problem(title: "Unable to create appointment.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Update(int id, [FromBody] AppointmentCreateUpdateDto dto)
    {
        if (id <= 0) return BadRequest("Invalid id.");
        if (dto is null) return BadRequest("Body is required.");
        if (dto.DoctorId <= 0 || dto.PatientId <= 0) return BadRequest("Valid DoctorId and PatientId are required.");

        try
        {
            var ok = service.Update(id, dto);
            return ok ? NoContent() : NotFound();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "FK not found when updating appointment {Id}", id);
            return BadRequest("Referenced doctor or patient not found.");
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation error updating appointment {Id}", id);
            return BadRequest("Invalid data.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating appointment {Id}", id);
            return Problem(title: "Unable to update appointment.", statusCode: StatusCodes.Status500InternalServerError);
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
            logger.LogError(ex, "Error deleting appointment {Id}", id);
            return Problem(title: "Unable to delete appointment.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
