using Hospital.Application.Contracts.Doctors;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DoctorsController(IDoctorService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DoctorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<DoctorDto>> GetAll()
        => Ok(service.GetAll());

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DoctorDto> Get(int id)
        => service.Get(id) is { } d ? Ok(d) : NotFound();

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DoctorDto> Create([FromBody] DoctorCreateUpdateDto dto)
    {
        var created = service.Create(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Update(int id, [FromBody] DoctorCreateUpdateDto dto)
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
