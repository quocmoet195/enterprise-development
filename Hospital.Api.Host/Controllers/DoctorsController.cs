using Hospital.Application.Contracts.Doctors;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController(IDoctorService service, ILogger<DoctorsController> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DoctorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<DoctorDto>> GetAll()
    {
        try
        {
            return Ok(service.GetAll());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all doctors");
            return Problem(title: "Unable to fetch doctors.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DoctorDto> Get(int id)
    {
        try
        {
            var dto = service.Get(id);
            return dto is null ? NotFound() : Ok(dto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting doctor by id {Id}", id);
            return Problem(title: "Unable to fetch doctor.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DoctorDto> Create([FromBody] DoctorCreateUpdateDto dto)
    {
        if (dto is null) return BadRequest("Body is required.");

        try
        {
            var created = service.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation error creating doctor");
            return BadRequest("Invalid data.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating doctor");
            return Problem(title: "Unable to create doctor.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Update(int id, [FromBody] DoctorCreateUpdateDto dto)
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
            logger.LogWarning(ex, "Validation error updating doctor {Id}", id);
            return BadRequest("Invalid data.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating doctor {Id}", id);
            return Problem(title: "Unable to update doctor.", statusCode: StatusCodes.Status500InternalServerError);
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
            logger.LogError(ex, "Error deleting doctor {Id}", id);
            return Problem(title: "Unable to delete doctor.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
