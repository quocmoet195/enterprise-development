using Hospital.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.Contracts.Patients;

/// <summary>
/// DTO used for creating or updating a patient.
/// </summary>
/// <param name="Passport">
/// The passport number of the patient.
/// </param>
/// <param name="FullName">
/// The full name of the patient.
/// </param>
/// <param name="Gender">
/// The gender of the patient (<see cref="Hospital.Domain.Enums.Gender"/>).
/// </param>
/// <param name="BirthDate">
/// The birth date of the patient.
/// </param>
/// <param name="Address">
/// The residential address of the patient.
/// </param>
/// <param name="BloodGroup">
/// The blood group of the patient (<see cref="Hospital.Domain.Enums.BloodGroup"/>).
/// </param>
/// <param name="Rhesus">
/// The rhesus factor of the patient (<see cref="Hospital.Domain.Enums.RhesusFactor"/>).
/// </param>
/// <param name="Phone">
/// The contact phone number of the patient.
/// </param>
public record PatientCreateUpdateDto(
    [Required, StringLength(50)] string Passport,
    [Required, StringLength(200)] string FullName,
    [Required] Gender Gender,
    [Required] DateOnly BirthDate,
    [Required, StringLength(300)] string Address,
    [Required] BloodGroup BloodGroup,
    [Required] RhesusFactor Rhesus,
    [Required, StringLength(50)] string Phone
);
