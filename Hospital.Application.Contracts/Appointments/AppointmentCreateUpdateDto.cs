using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.Contracts.Appointments;

/// <summary>
/// DTO for creating or updating an appointment record.
/// </summary>
/// <param name="StartAt">
/// The date and time when the appointment is scheduled to start.
/// Can be <c>null</c> if not yet determined.
/// </param>
/// <param name="RoomNumber">
/// The room where the appointment takes place.
/// </param>
/// <param name="IsFollowUp">
/// Indicates whether this appointment is a follow-up visit.
/// </param>
/// <param name="DoctorId">
/// The unique identifier of the doctor assigned to this appointment.
/// </param>
/// <param name="PatientId">
/// The unique identifier of the patient attending the appointment.
/// </param>
public record AppointmentCreateUpdateDto(
    [Required] DateTime StartAt,
    [Required, StringLength(20)] string RoomNumber,
    bool IsFollowUp,
    [Range(1, int.MaxValue)] int DoctorId,
    [Range(1, int.MaxValue)] int PatientId
);
