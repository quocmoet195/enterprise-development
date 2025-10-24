using Hospital.Application.Contracts;
using Hospital.Application.Contracts.Appointments;
using Hospital.Application.Contracts.Doctors;
using Hospital.Application.Contracts.Patients;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(IAnalyticsService service, ILogger<AnalyticsController> logger) : ControllerBase
{
    [HttpGet("doctors/10plus")]
    public ActionResult<IEnumerable<DoctorDto>> Doctors10Plus()
    {
        try
        {
            return Ok(service.GetDoctorsWith10Plus());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving doctors with 10+ years experience");
            return BadRequest("An error occurred while processing the request to get doctors with 10+ years of experience");
        }
    }

    [HttpGet("doctor/{id:int}/patients")]
    public ActionResult<IEnumerable<string>> PatientNamesForDoctor(int id)
    {
        if (id <= 0)
            return BadRequest("Doctor ID must be positive.");

        try
        {
            var result = service.GetPatientNamesForDoctor(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching patient names for doctor {DoctorId}", id);
            return Problem(title: "Unable to fetch patient names.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("followups/last-month")]
    public ActionResult<int> FollowUpsLastMonth([FromQuery] DateTime? now)
    {
        var timestamp = now ?? DateTime.UtcNow;

        try
        {
            return Ok(service.GetFollowUpsLastMonth(timestamp));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error counting follow-ups for the last month (timestamp: {Timestamp})", timestamp);
            return Problem(title: "Unable to fetch follow-up data.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("patients/30plus-multi-doctors")]
    public ActionResult<IEnumerable<PatientDto>> Patients30PlusMultiDoctors([FromQuery] DateTime? today)
    {
        var date = DateOnly.FromDateTime((today ?? DateTime.UtcNow).Date);

        try
        {
            return Ok(service.GetPatients30PlusMultiDoctors(date));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving patients 30+ with multiple doctors (today={Today})", date);
            return Problem(title: "Unable to fetch analytics.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("appointments/this-month")]
    public ActionResult<IEnumerable<AppointmentDto>> ThisMonthInRoom([FromQuery] string room, [FromQuery] DateTime? now)
    {
        if (string.IsNullOrWhiteSpace(room))
            return BadRequest("Room parameter is required.");

        var timestamp = now ?? DateTime.UtcNow;

        try
        {
            return Ok(service.GetThisMonthInRoom(room, timestamp));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving appointments for room {Room} at {Now}", room, timestamp);
            return Problem(title: "Unable to fetch room appointments.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
