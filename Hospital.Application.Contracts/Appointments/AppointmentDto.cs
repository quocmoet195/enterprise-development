namespace Hospital.Application.Contracts.Appointments;

/// <summary>
/// DTO representing an appointment returned from the API.
/// </summary>
/// <param name="Id">
/// Unique identifier of the appointment.
/// </param>
/// <param name="StartAt">
/// The scheduled start date and time of the appointment.
/// Can be <c>null</c> if not yet determined.
/// </param>
/// <param name="RoomNumber">
/// The room number where the appointment takes place.
/// </param>
/// <param name="IsFollowUp">
/// Indicates whether this appointment is a follow-up visit.
/// </param>
/// <param name="DoctorId">
/// The unique identifier of the doctor assigned to the appointment.
/// </param>
/// <param name="DoctorName">
/// The full name of the doctor assigned to this appointment.
/// </param>
/// <param name="PatientId">
/// The unique identifier of the patient who has the appointment.
/// </param>
/// <param name="PatientName">
/// The full name of the patient who has the appointment.
/// </param>
public record AppointmentDto(
    int Id,
    DateTime? StartAt,
    string? RoomNumber,
    bool IsFollowUp,
    int DoctorId,
    string DoctorName,
    int PatientId,
    string PatientName
);
