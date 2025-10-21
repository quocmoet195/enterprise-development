using Hospital.Application.Contracts.Patients;

namespace Hospital.Application.Contracts;

/// <summary>
/// Application service that provides CRUD operations for patients.
/// </summary>
public interface IPatientService 
{
    /// <summary>
    /// Returns all patients.
    /// </summary>
    /// <returns>A sequence of <see cref="PatientDto"/> representing all patients.</returns>
    IEnumerable<PatientDto> GetAll();

    /// <summary>
    /// Returns a single patient by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the patient.</param>
    /// <returns>
    /// The <see cref="PatientDto"/> if found; otherwise, <c>null</c>.
    /// </returns>
    PatientDto? Get(int id);

    /// <summary>
    /// Creates a new patient record.
    /// </summary>
    /// <param name="input">The data used to create the patient.</param>
    /// <returns>The created <see cref="PatientDto"/> instance.</returns>
    PatientDto Create(PatientCreateUpdateDto input);

    /// <summary>
    /// Updates an existing patient record.
    /// </summary>
    /// <param name="id">The unique identifier of the patient.</param>
    /// <param name="input">The updated data for the patient.</param>
    /// <returns>
    /// <c>true</c> if the patient was successfully updated; otherwise, <c>false</c>.
    /// </returns>
    bool Update(int id, PatientCreateUpdateDto input);

    /// <summary>
    /// Deletes a patient record by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the patient.</param>
    /// <returns>
    /// <c>true</c> if the patient was successfully deleted; otherwise, <c>false</c>.
    /// </returns>
    bool Delete(int id);
}
