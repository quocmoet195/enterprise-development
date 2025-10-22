using AutoMapper;
using Hospital.Application.Contracts;
using Hospital.Application.Contracts.Patients;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;

namespace Hospital.Application.Services;

/// <summary>
/// Provides CRUD operations for managing patient data in the hospital system.
/// </summary>
public class PatientService(IPatientRepository repo, IMapper mapper) : IPatientService
{
    /// <summary>
    /// Retrieves all patients from the repository.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="PatientDto"/> objects representing all patients.
    /// </returns>
    public IEnumerable<PatientDto> GetAll()
        => repo.GetAll().Select(mapper.Map<PatientDto>);

    /// <summary>
    /// Retrieves a specific patient by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the patient to retrieve.</param>
    /// <returns>
    /// A <see cref="PatientDto"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public PatientDto? Get(int id)
        => repo.Get(id) is { } p ? mapper.Map<PatientDto>(p) : null;

    /// <summary>
    /// Creates a new patient record based on the provided input data.
    /// </summary>
    /// <param name="input">The data transfer object containing patient information.</param>
    /// <returns>
    /// A <see cref="PatientDto"/> representing the created patient.
    /// </returns>
    public PatientDto Create(PatientCreateUpdateDto input)
    {
        var entity = mapper.Map<Patient>(input);

        var created = repo.Add(entity);
        return mapper.Map<PatientDto>(created);
    }

    /// <summary>
    /// Updates an existing patient's data by their ID.
    /// </summary>
    /// <param name="id">The ID of the patient to update.</param>
    /// <param name="input">The DTO containing updated patient information.</param>
    /// <returns>
    /// <c>true</c> if the update was successful; otherwise, <c>false</c>.
    /// </returns>
    public bool Update(int id, PatientCreateUpdateDto input)
    {
        var entity = repo.Get(id);
        if (entity is null) return false;

        mapper.Map(input, entity);

        return repo.Update(entity);
    }

    /// <summary>
    /// Deletes a patient record by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the patient to delete.</param>
    /// <returns>
    /// <c>true</c> if the patient was deleted successfully; otherwise, <c>false</c>.
    /// </returns>
    public bool Delete(int id)
        => repo.Delete(id);
}
