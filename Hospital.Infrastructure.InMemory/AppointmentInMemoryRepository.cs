using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Tests;
using System.Data;

namespace Hospital.Infrastructure.InMemory;

/// <summary>
/// In-memory repository for managing <see cref="Appointment"/> entities.
/// Uses <see cref="TestData"/> as a simple data source for testing and development.
/// </summary>
public class AppointmentInMemoryRepository(TestData _seed) : IAppointmentRepository
{
    /// <summary>
    /// Retrieves all appointments from the in-memory collection, ordered by their ID.
    /// </summary>
    /// <returns>A collection of all <see cref="Appointment"/> objects.</returns>
    public IEnumerable<Appointment> GetAll() => _seed.Appointments.OrderBy(x => x.Id);

    /// <summary>
    /// Retrieves a single appointment by its unique identifier.
    /// </summary>
    /// <param name="id">The appointment ID.</param>
    /// <returns>
    /// The matching <see cref="Appointment"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public Appointment? Get(int id) => _seed.Appointments.FirstOrDefault(x => x.Id == id);

    /// <summary>
    /// Adds a new appointment to the in-memory collection.
    /// The appointment ID is automatically generated.
    /// </summary>
    /// <param name="a">The <see cref="Appointment"/> instance to add.</param>
    /// <returns>The added <see cref="Appointment"/> with its assigned ID.</returns>
    public Appointment Add(Appointment a)
    {
        a.Id = (_seed.Appointments.LastOrDefault()?.Id ?? 0) + 1;
        _seed.Appointments.Add(a);
        return a;
    }

    /// <summary>
    /// Updates an existing appointment in the collection.
    /// </summary>
    /// <param name="a">The <see cref="Appointment"/> instance with updated values.</param>
    /// <returns>
    /// <c>true</c> if the appointment was successfully updated; otherwise, <c>false</c>.
    /// </returns>
    public bool Update(Appointment a)
    {
        var idx = _seed.Appointments.FindIndex(x => x.Id == a.Id);
        if (idx < 0) return false;
        _seed.Appointments[idx] = a;
        return true;
        }

    /// <summary>
    /// Deletes an appointment from the collection by its ID.
    /// </summary>
    /// <param name="id">The ID of the appointment to delete.</param>
    /// <returns>
    /// <c>true</c> if the appointment was found and removed; otherwise, <c>false</c>.
    /// </returns>
    public bool Delete(int id)
    {
        var e = Get(id);
        return e != null && _seed.Appointments.Remove(e);
    }
}