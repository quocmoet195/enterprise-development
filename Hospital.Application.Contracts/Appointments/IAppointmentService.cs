using Hospital.Application.Contracts.Appointments;

namespace Hospital.Application.Contracts;

/// <summary>
/// Application service that provides CRUD operations and queries for appointments.
/// </summary>
public interface IAppointmentService 
{
    /// <summary>
    /// Returns all appointments.
    /// </summary>
    /// <returns>A sequence of <see cref="AppointmentDto"/>.</returns>
    IEnumerable<AppointmentDto> GetAll();

    /// <summary>
    /// Returns a single appointment by its identifier.
    /// </summary>
    /// <param name="id">The appointment identifier.</param>
    /// <returns>
    /// The <see cref="AppointmentDto"/> if found; otherwise, <c>null</c>.
    /// </returns>
    AppointmentDto? Get(int id);

    /// <summary>
    /// Creates a new appointment.
    /// </summary>
    /// <param name="input">The appointment data to create.</param>
    /// <returns>The created <see cref="AppointmentDto"/>.</returns>
    AppointmentDto Create(AppointmentCreateUpdateDto input);

    /// <summary>
    /// Updates an existing appointment.
    /// </summary>
    /// <param name="id">The appointment identifier.</param>
    /// <param name="input">The updated appointment data.</param>
    /// <returns>
    /// <c>true</c> if the appointment was updated; otherwise, <c>false</c>.
    /// </returns>
    bool Update(int id, AppointmentCreateUpdateDto input);

    /// <summary>
    /// Deletes an appointment by its identifier.
    /// </summary>
    /// <param name="id">The appointment identifier.</param>
    /// <returns>
    /// <c>true</c> if the appointment was deleted; otherwise, <c>false</c>.
    /// </returns>
    bool Delete(int id);
}
