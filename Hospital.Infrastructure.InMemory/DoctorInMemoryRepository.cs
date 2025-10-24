using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.InMemory.Seed;

namespace Hospital.Infrastructure.InMemory;

/// <summary>
/// In-memory repository for managing <see cref="Doctor"/> entities.
/// Uses <see cref="TestData"/> as a data source for testing and development.
/// </summary>
public class DoctorInMemoryRepository(InMemoryData seed) : IDoctorRepository
{
    /// <summary>
    /// Retrieves all doctors from the in-memory collection, ordered by their ID.
    /// </summary>
    /// <returns>A collection of all <see cref="Doctor"/> objects.</returns>
    public IEnumerable<Doctor> GetAll() => seed.Doctors.OrderBy(x => x.Id);

    /// <summary>
    /// Retrieves a single doctor by their unique identifier.
    /// </summary>
    /// <param name="id">The doctor's ID.</param>
    /// <returns>
    /// The matching <see cref="Doctor"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public Doctor? Get(int id) => seed.Doctors.FirstOrDefault(x => x.Id == id);

    /// <summary>
    /// Adds a new doctor to the in-memory collection.
    /// The doctor ID is automatically generated.
    /// </summary>
    /// <param name="d">The <see cref="Doctor"/> instance to add.</param>
    /// <returns>The added <see cref="Doctor"/> with its assigned ID.</returns>
    public Doctor Add(Doctor d)
    {
        d.Id = (seed.Patients.Count != 0 ? seed.Patients.Max(x => x.Id) : 0) + 1;
        seed.Doctors.Add(d);
        return d;
    }

    /// <summary>
    /// Updates an existing doctor in the collection.
    /// </summary>
    /// <param name="d">The <see cref="Doctor"/> instance with updated values.</param>
    /// <returns>
    /// <c>true</c> if the doctor was successfully updated; otherwise, <c>false</c>.
    /// </returns>
    public bool Update(Doctor d)
    {
        var idx = seed.Doctors.FindIndex(x => x.Id == d.Id);
        if (idx < 0) return false;
        seed.Doctors[idx] = d;
        return true;
    }

    /// <summary>
    /// Deletes a doctor from the collection by their ID.
    /// </summary>
    /// <param name="id">The ID of the doctor to delete.</param>
    /// <returns>
    /// <c>true</c> if the doctor was found and removed; otherwise, <c>false</c>.
    /// </returns>
    public bool Delete(int id)
    {
        var e = Get(id);
        return e != null && seed.Doctors.Remove(e);
    }
}
