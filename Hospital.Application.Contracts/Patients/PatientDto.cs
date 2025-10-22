namespace Hospital.Application.Contracts.Patients;

/// <summary>
/// DTO representing a patient returned from the API.
/// </summary>
/// <param name="Id">
/// The unique identifier of the patient.
/// </param>
/// <param name="FullName">
/// The full name of the patient.
/// </param>
/// <param name="Gender">
/// The gender of the patient, represented as a string (e.g., "Male", "Female").
/// </param>
/// <param name="BirthDate">
/// The birth date of the patient.
/// </param>
/// <param name="Address">
/// The residential address of the patient.
/// </param>
/// <param name="BloodGroup">
/// The blood group of the patient, represented as a string (e.g., "A", "B", "AB", "O").
/// </param>
/// <param name="Rhesus">
/// The rhesus factor of the patient, represented as a string (e.g., "Positive", "Negative").
/// </param>
/// <param name="Phone">
/// The contact phone number of the patient.
/// </param>
public record PatientDto(
    int Id,
    string FullName,
    string Gender,
    DateOnly BirthDate,
    string Address,
    string BloodGroup,
    string Rhesus,
    string Phone
);
