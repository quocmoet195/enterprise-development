using Microsoft.AspNetCore.Mvc;
using Hospital.Application.Contracts;
using Hospital.Application.Contracts.Appointments;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Hospital.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AppointmentsController(IAppointmentService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<AppointmentDto>> GetAll()
        => Ok(service.GetAll());

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<AppointmentDto> Get(int id)
        => service.Get(id) is { } d ? Ok(d) : NotFound();

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]                 
    [ProducesResponseType(StatusCodes.Status404NotFound)]                   
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<AppointmentDto> Create([FromBody] AppointmentCreateUpdateDto dto)
    {
        try
        {
            var created = service.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]                 
    [ProducesResponseType(StatusCodes.Status404NotFound)]                   
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Update(int id, [FromBody] AppointmentCreateUpdateDto dto)
    {
        var ok = service.Update(id, dto);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Delete(int id)
    {
        var ok = service.Delete(id);
        return ok ? NoContent() : NotFound();
    }
}