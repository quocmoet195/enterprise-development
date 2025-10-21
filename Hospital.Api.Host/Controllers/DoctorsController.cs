using Hospital.Application.Contracts;
using Hospital.Application.Contracts.Doctors;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController(IDoctorService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<DoctorDto>> GetAll() => Ok(service.GetAll());

    [HttpGet("{id:int}")]
    public ActionResult<DoctorDto> Get(int id)
        => service.Get(id) is { } d ? Ok(d) : NotFound();

    [HttpPost]
    public ActionResult<DoctorDto> Create([FromBody] DoctorCreateUpdateDto input)
    {
        var created = service.Create(input);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] DoctorCreateUpdateDto input)
        => service.Update(id, input) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
        => service.Delete(id) ? NoContent() : NotFound();
}
