
namespace Hospital.Application.Contracts.Doctors;

/// <summary>
/// Application service that provides CRUD operations for doctors.
/// </summary>
public interface IDoctorService
{
    /// <summary>
    /// Returns all doctors.
    /// </summary>
    /// <returns>A sequence of <see cref="DoctorDto"/>.</returns>
    public IEnumerable<DoctorDto> GetAll();

    /// <summary>
    /// Returns a single doctor by its identifier.
    /// </summary>
    /// <param name="id">The doctor identifier.</param>
    /// <returns>
    /// The <see cref="DoctorDto"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public DoctorDto? Get(int id);

    /// <summary>
    /// Creates a new doctor.
    /// </summary>
    /// <param name="input">The doctor data to create.</param>
    /// <returns>The created <see cref="DoctorDto"/>.</returns>
    public DoctorDto Create(DoctorCreateUpdateDto input);

    /// <summary>
    /// Updates an existing doctor.
    /// </summary>
    /// <param name="id">The doctor identifier.</param>
    /// <param name="input">The updated doctor data.</param>
    /// <returns>
    /// <c>true</c> if the doctor was updated; otherwise, <c>false</c>.
    /// </returns>
    public bool Update(int id, DoctorCreateUpdateDto input);

    /// <summary>
    /// Deletes a doctor by its identifier.
    /// </summary>
    /// <param name="id">The doctor identifier.</param>
    /// <returns>
    /// <c>true</c> if the doctor was deleted; otherwise, <c>false</c>.
    /// </returns>
    public bool Delete(int id);
}
