using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.EF.Repositories;

/// <summary>
/// Entity Framework Core repository implementation for managing <see cref="Doctor"/> entities.
/// Provides CRUD (Create, Read, Update, Delete) operations using the <see cref="HospitalDbContext"/>.
/// </summary>
public class DoctorEfRepository(HospitalDbContext db) : IDoctorRepository
{
    /// <summary>
    /// Retrieves all doctors from the database in ascending order by their identifier.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Doctor"/> entities.
    /// </returns>
    public IEnumerable<Doctor> GetAll() =>
        [.. db.Doctors.AsNoTracking().OrderBy(x => x.Id)];


    /// <summary>
    /// Retrieves a specific doctor by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor.</param>
    /// <returns>
    /// The corresponding <see cref="Doctor"/> entity if found; otherwise, <see langword="null"/>.
    /// </returns>
    public Doctor? Get(int id) => db.Doctors.Find(id);

    /// <summary>
    /// Adds a new <see cref="Doctor"/> entity to the database.
    /// </summary>
    /// <param name="d">The <see cref="Doctor"/> entity to add.</param>
    /// <returns>
    /// The added <see cref="Doctor"/> entity (with its generated ID).
    /// </returns>
    public Doctor Add(Doctor d)
    {
        db.Doctors.Add(d);
        db.SaveChanges();
        return d;
    }

    /// <summary>
    /// Updates an existing <see cref="Doctor"/> record in the database.
    /// </summary>
    /// <param name="d">The modified <see cref="Doctor"/> entity.</param>
    /// <returns>
    /// <see langword="true"/> if the entity was found and updated; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Update(Doctor d)
    {
        if (!db.Doctors.Any(x => x.Id == d.Id)) return false;
        db.Doctors.Update(d);
        db.SaveChanges();
        return true;
    }

    /// <summary>
    /// Deletes an existing <see cref="Doctor"/> by their unique identifier.
    /// </summary>
    /// <param name="id">The identifier of the doctor to delete.</param>
    /// <returns>
    /// <see langword="true"/> if the entity was found and deleted; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Delete(int id)
    {
        var e = db.Doctors.Find(id);
        if (e is null) return false;
        db.Doctors.Remove(e);
        db.SaveChanges();
        return true;
    }
}
