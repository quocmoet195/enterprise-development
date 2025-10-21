namespace Hospital.Application.Contracts.Doctors;

/// <summary>
/// DTO representing a doctor returned from the API.
/// </summary>
/// <param name="Id">
/// Unique identifier of the doctor.
/// </param>
/// <param name="FullName">
/// The full name of the doctor.
/// </param>
/// <param name="BirthYear">
/// The year of birth of the doctor.
/// </param>
/// <param name="ExperienceYears">
/// The total number of years of professional experience the doctor has.
/// </param>
/// <param name="Specialization">
/// The medical specialization of the doctor (e.g., "Therapist", "Surgeon").
/// </param>
public record DoctorDto(
    int Id,
    string FullName,
    int BirthYear,
    int ExperienceYears,
    string Specialization
);
