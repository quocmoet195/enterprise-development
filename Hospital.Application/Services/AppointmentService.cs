using AutoMapper;
using Hospital.Application.Contracts.Appointments;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;

namespace Hospital.Application.Services;

/// <summary>
/// Service responsible for managing appointments, including creation, updates, and retrieval.
/// </summary>
/// <remarks>
/// Works with in-memory repositories for doctors and patients.  
/// Uses <see cref="AutoMapper"/> to map between domain entities and DTOs.
/// </remarks>
public class AppointmentService(
    IAppointmentRepository repo,
    IPatientRepository patients,
    IDoctorRepository doctors,
    IMapper mapper) : IAppointmentService
{
    /// <summary>
    /// Retrieves all appointments in the system.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="AppointmentDto"/> representing all appointments.
    /// </returns>
    public IEnumerable<AppointmentDto> GetAll()
        => repo.GetAll().Select(mapper.Map<AppointmentDto>);

    /// <summary>
    /// Retrieves a specific appointment by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the appointment to retrieve.</param>
    /// <returns>
    /// The corresponding <see cref="AppointmentDto"/> if found, otherwise <c>null</c>.
    /// </returns>
    public AppointmentDto? Get(int id)
        => repo.Get(id) is { } d ? mapper.Map<AppointmentDto>(d) : null;

    /// <summary>
    /// Creates a new appointment by linking a doctor and patient at a specific time and room.
    /// </summary>
    /// <param name="input">The appointment data transfer object containing appointment details.</param>
    /// <returns>
    /// The created <see cref="AppointmentDto"/> with generated ID and mapped fields.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when the provided <paramref name="input.PatientId"/> or <paramref name="input.DoctorId"/> does not exist.
    /// </exception>
    public AppointmentDto Create(AppointmentCreateUpdateDto input)
    {
        var patient = patients.Get(input.PatientId)
                     ?? throw new KeyNotFoundException($"Patient {input.PatientId} not found");
        var doctor = doctors.Get(input.DoctorId)
                     ?? throw new KeyNotFoundException($"Doctor {input.DoctorId} not found");

        var entity = mapper.Map<Appointment>(input);
        entity.Doctor = doctor;
        entity.Patient = patient;

        var created = repo.Add(entity);

        return mapper.Map<AppointmentDto>(created);
    }

    /// <summary>
    /// Updates an existing appointment with new data.
    /// </summary>
    /// <param name="id">The ID of the appointment to update.</param>
    /// <param name="input">The appointment update DTO containing new values.</param>
    /// <returns>
    /// <c>true</c> if the update was successful; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when the provided <paramref name="input.PatientId"/> or <paramref name="input.DoctorId"/> does not exist.
    /// </exception>
    public bool Update(int id, AppointmentCreateUpdateDto input)
    {
        var entity = repo.Get(id);
        if (entity is null) return false;

        var patient = patients.Get(input.PatientId)
                     ?? throw new KeyNotFoundException($"Patient {input.PatientId} not found");
        var doctor = doctors.Get(input.DoctorId)
                     ?? throw new KeyNotFoundException($"Doctor {input.DoctorId} not found");

        mapper.Map(input, entity);
        entity.Doctor = doctor;
        entity.Patient = patient;

        return repo.Update(entity);
    }

    /// <summary>
    /// Deletes an existing appointment by its ID.
    /// </summary>
    /// <param name="id">The ID of the appointment to delete.</param>
    /// <returns>
    /// <c>true</c> if the appointment was deleted successfully; otherwise, <c>false</c>.
    /// </returns>
    public bool Delete(int id) => repo.Delete(id);
}
