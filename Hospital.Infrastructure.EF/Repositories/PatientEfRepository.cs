using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.EF.Repositories;

/// <summary>
/// Entity Framework Core repository implementation for managing <see cref="Patient"/> entities.
/// Provides CRUD (Create, Read, Update, Delete) operations through <see cref="HospitalDbContext"/>.
/// </summary>
public class PatientEfRepository(HospitalDbContext db) : IPatientRepository
{
    /// <summary>
    /// Retrieves all patients from the database, ordered by their identifier.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Patient"/> entities.
    /// </returns>
    public IEnumerable<Patient> GetAll() =>
        [.. db.Patients.AsNoTracking().OrderBy(x => x.Id)];

    /// <summary>
    /// Retrieves a specific patient by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the patient.</param>
    /// <returns>
    /// The corresponding <see cref="Patient"/> entity if found; otherwise, <see langword="null"/>.
    /// </returns>
    public Patient? Get(int id) => db.Patients.Find(id);

    /// <summary>
    /// Adds a new <see cref="Patient"/> record to the database.
    /// </summary>
    /// <param name="d">The <see cref="Patient"/> entity to add.</param>
    /// <returns>
    /// The added <see cref="Patient"/> entity (with its generated ID).
    /// </returns>
    public Patient Add(Patient d)
    {
        db.Patients.Add(d);
        db.SaveChanges();
        return d;
    }

    /// <summary>
    /// Updates an existing <see cref="Patient"/> record in the database.
    /// </summary>
    /// <param name="d">The modified <see cref="Patient"/> entity.</param>
    /// <returns>
    /// <see langword="true"/> if the update succeeded; otherwise, <see langword="false"/> if no matching entity was found.
    /// </returns>
    public bool Update(Patient d)
    {
        if (!db.Patients.Any(x => x.Id == d.Id)) return false;
        db.Patients.Update(d);
        db.SaveChanges();
        return true;
    }

    /// <summary>
    /// Deletes a <see cref="Patient"/> record by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the patient to delete.</param>
    /// <returns>
    /// <see langword="true"/> if the entity was found and deleted; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Delete(int id)
    {
        var e = db.Patients.Find(id);
        if (e is null) return false;
        db.Patients.Remove(e);
        db.SaveChanges();
        return true;
    }
}
