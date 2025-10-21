using Hospital.Application.Contracts;
using Hospital.Application.Contracts.Appointments;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(IAppointmentService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<AppointmentDto>> GetAll() => Ok(service.GetAll());

    [HttpGet("{id:int}")]
    public ActionResult<AppointmentDto> Get(int id)
        => service.Get(id) is { } d ? Ok(d) : NotFound();

    [HttpPost]
    public ActionResult<AppointmentDto> Create([FromBody] AppointmentCreateUpdateDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var created = service.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] AppointmentCreateUpdateDto input)
        => service.Update(id, input) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
        => service.Delete(id) ? NoContent() : NotFound();
}

