using Hospital.Application.Contracts;
using Hospital.Application.Contracts.Patients;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(IPatientService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<PatientDto>> GetAll() => Ok(service.GetAll());

    [HttpGet("{id:int}")]
    public ActionResult<PatientDto> Get(int id)
        => service.Get(id) is { } d ? Ok(d) : NotFound();

    [HttpPost]
    public ActionResult<PatientDto> Create([FromBody] PatientCreateUpdateDto input)
    {
        var created = service.Create(input);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] PatientCreateUpdateDto input)
        => service.Update(id, input) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
        => service.Delete(id) ? NoContent() : NotFound();
}
