using AutoMapper;
using Hospital.Application.Contracts;
using Hospital.Application.Contracts.Doctors;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;
using Hospital.Infrastructure.InMemory;

namespace Hospital.Application.Services;

/// <summary>
/// Provides operations for managing doctor data, including CRUD functionality.
/// </summary>
public class DoctorService(DoctorInMemoryRepository repo, IMapper mapper) : IDoctorService
{
    /// <summary>
    /// Retrieves all doctors available in the system.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="DoctorDto"/> objects representing all doctors.
    /// </returns>
    public IEnumerable<DoctorDto> GetAll()
        => repo.GetAll().Select(mapper.Map<DoctorDto>);

    /// <summary>
    /// Retrieves a doctor by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the doctor to retrieve.</param>
    /// <returns>
    /// The corresponding <see cref="DoctorDto"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public DoctorDto? Get(int id)
        => repo.Get(id) is { } d ? mapper.Map<DoctorDto>(d) : null;

    /// <summary>
    /// Creates a new doctor entity based on the provided input data.
    /// </summary>
    /// <param name="input">The DTO containing doctor creation details.</param>
    /// <returns>
    /// A <see cref="DoctorDto"/> representing the newly created doctor.
    /// </returns>
    /// <remarks>
    /// The <see cref="DoctorSpecialization"/> value is parsed from a string; 
    /// if parsing fails, it defaults to <see cref="DoctorSpecialization.Other"/>.
    /// </remarks>
    public DoctorDto Create(DoctorCreateUpdateDto input)
    {
        var spec = Enum.TryParse<DoctorSpecialization>(input.Specialization, true, out var s)
            ? s : DoctorSpecialization.Other;

        var created = repo.Add(new Doctor
        {
            FullName = input.FullName,
            BirthYear = input.BirthYear,
            ExperienceYears = input.ExperienceYears,
            Specialization = spec,
            Passport = "" // auto-generated or placeholder
        });

        return mapper.Map<DoctorDto>(created);
    }

    /// <summary>
    /// Updates an existing doctor’s data by their ID.
    /// </summary>
    /// <param name="id">The ID of the doctor to update.</param>
    /// <param name="input">The DTO containing updated doctor information.</param>
    /// <returns>
    /// <c>true</c> if the update was successful; otherwise, <c>false</c>.
    /// </returns>
    public bool Update(int id, DoctorCreateUpdateDto input)
    {
        var entity = repo.Get(id);
        if (entity is null) return false;

        entity.FullName = input.FullName;
        entity.BirthYear = input.BirthYear;
        entity.ExperienceYears = input.ExperienceYears;
        entity.Specialization = Enum.TryParse<DoctorSpecialization>(input.Specialization, true, out var s)
            ? s : DoctorSpecialization.Other;

        return repo.Update(entity);
    }

    /// <summary>
    /// Deletes a doctor from the repository by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the doctor to delete.</param>
    /// <returns>
    /// <c>true</c> if the doctor was successfully deleted; otherwise, <c>false</c>.
    /// </returns>
    public bool Delete(int id)
        => repo.Delete(id);
}
