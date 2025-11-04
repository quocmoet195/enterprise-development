using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.EF.Repositories;

/// <summary>
/// Entity Framework Core repository implementation for managing <see cref="Appointment"/> entities.
/// Provides CRUD operations using the <see cref="HospitalDbContext"/>.
/// </summary>
public class AppointmentEfRepository(HospitalDbContext db) : IAppointmentRepository
{
    /// <summary>
    /// Retrieves all appointments from the database, including related <see cref="Patient"/> and <see cref="Doctor"/> entities.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Appointment"/> objects ordered by their identifier.
    /// </returns>
    public IEnumerable<Appointment> GetAll() =>
        db.Appointments
          .Include(a => a.Patient)
          .Include(a => a.Doctor)
          .AsNoTracking()
          .OrderBy(a => a.Id)
          .ToList();

    /// <summary>
    /// Retrieves a specific appointment by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment.</param>
    /// <returns>
    /// The matching <see cref="Appointment"/> if found; otherwise, <see langword="null"/>.
    /// </returns>
    public Appointment? Get(int id) => db.Appointments.Find(id);

    /// <summary>
    /// Adds a new <see cref="Appointment"/> entity to the database.
    /// </summary>
    /// <param name="d">The <see cref="Appointment"/> entity to add.</param>
    /// <returns>
    /// The added <see cref="Appointment"/> entity (with its generated ID).
    /// </returns>
    public Appointment Add(Appointment d)
    {
        db.Appointments.Add(d);
        db.SaveChanges();
        return d;
    }

    /// <summary>
    /// Updates an existing <see cref="Appointment"/> record in the database.
    /// </summary>
    /// <param name="d">The modified <see cref="Appointment"/> entity.</param>
    /// <returns>
    /// <see langword="true"/> if the update succeeded; otherwise, <see langword="false"/> (if entity not found).
    /// </returns>
    public bool Update(Appointment d)
    {
        if (!db.Appointments.Any(x => x.Id == d.Id)) return false;
        db.Appointments.Update(d);
        db.SaveChanges();
        return true;
    }

    /// <summary>
    /// Deletes an existing <see cref="Appointment"/> by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment to delete.</param>
    /// <returns>
    /// <see langword="true"/> if the entity was found and deleted; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Delete(int id)
    {
        var e = db.Appointments.Find(id);
        if (e is null) return false;
        db.Appointments.Remove(e);
        db.SaveChanges();
        return true;
    }
}
