using Hospital.Application.Contracts;
using Hospital.Application.Contracts.Appointments;
using Hospital.Application.Contracts.Doctors;
using Hospital.Application.Contracts.Patients;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(IAnalyticsService service) : ControllerBase
{
    [HttpGet("doctors/10plus")]
    public ActionResult<IEnumerable<DoctorDto>> Doctors10Plus()
        => Ok(service.GetDoctorsWith10Plus());

    [HttpGet("doctor/{id:int}/patients")]
    public ActionResult<IEnumerable<string>> PatientNamesForDoctor(int id)
        => Ok(service.GetPatientNamesForDoctor(id));

    [HttpGet("followups/last-month")]
    public ActionResult<int> FollowUpsLastMonth([FromQuery] DateTime? now)
        => Ok(service.GetFollowUpsLastMonth(now ?? DateTime.UtcNow));

    [HttpGet("patients/30plus-multi-doctors")]
    public ActionResult<IEnumerable<PatientDto>> Patients30PlusMultiDoctors([FromQuery] DateTime? today)
    {
        var t = DateOnly.FromDateTime((today ?? DateTime.UtcNow).Date);
        return Ok(service.GetPatients30PlusMultiDoctors(t));
    }

    [HttpGet("appointments/this-month")]
    public ActionResult<IEnumerable<AppointmentDto>> ThisMonthInRoom([FromQuery] string room, [FromQuery] DateTime? now)
        => Ok(service.GetThisMonthInRoom(room, now ?? DateTime.UtcNow));
}
