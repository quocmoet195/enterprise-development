using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.InMemory.Seed;

namespace Hospital.Infrastructure.InMemory;

/// <summary>
/// In-memory repository for managing <see cref="Patient"/> entities.
/// Uses <see cref="TestData"/> as a data source for testing and development.
/// </summary>
public class PatientInMemoryRepository(InMemoryData seed) : IPatientRepository
{
    /// <summary>
    /// Retrieves all patients from the in-memory collection, ordered by their ID.
    /// </summary>
    /// <returns>A collection of all <see cref="Patient"/> objects.</returns>
    public IEnumerable<Patient> GetAll() => seed.Patients.OrderBy(x => x.Id);

    /// <summary>
    /// Retrieves a single patient by their unique identifier.
    /// </summary>
    /// <param name="id">The patient's ID.</param>
    /// <returns>
    /// The matching <see cref="Patient"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public Patient? Get(int id) => seed.Patients.FirstOrDefault(x => x.Id == id);

    /// <summary>
    /// Adds a new patient to the in-memory collection.
    /// The patient ID is automatically generated.
    /// </summary>
    /// <param name="p">The <see cref="Patient"/> instance to add.</param>
    /// <returns>The added <see cref="Patient"/> with its assigned ID.</returns>
    public Patient Add(Patient p)
    {
        p.Id = (seed.Patients.Count != 0 ? seed.Patients.Max(x => x.Id) : 0) + 1;
        seed.Patients.Add(p);
        return p;
    }

    /// <summary>
    /// Updates an existing patient in the collection.
    /// </summary>
    /// <param name="p">The <see cref="Patient"/> instance with updated values.</param>
    /// <returns>
    /// <c>true</c> if the patient was successfully updated; otherwise, <c>false</c>.
    /// </returns>
    public bool Update(Patient p)
    {
        var idx = seed.Patients.FindIndex(x => x.Id == p.Id);
        if (idx < 0) return false;
        seed.Patients[idx] = p;
        return true;
    }

    /// <summary>
    /// Deletes a patient from the collection by their ID.
    /// </summary>
    /// <param name="id">The ID of the patient to delete.</param>
    /// <returns>
    /// <c>true</c> if the patient was found and removed; otherwise, <c>false</c>.
    /// </returns>
    public bool Delete(int id)
    {
        var e = Get(id);
        return e != null && seed.Patients.Remove(e);
    }
}
