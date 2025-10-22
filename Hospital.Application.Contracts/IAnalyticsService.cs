using Hospital.Application.Contracts.Patients;
using Hospital.Application.Contracts.Doctors;
using Hospital.Application.Contracts.Appointments;

namespace Hospital.Application.Contracts;

/// <summary>
/// Provides analytical operations based on hospital data.
/// Returns aggregated and filtered data for reporting purposes.
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Retrieves all doctors with at least 10 years of experience.
    /// </summary>
    /// <returns>A collection of <see cref="DoctorDto"/> objects.</returns>
    public IEnumerable<DoctorDto> GetDoctorsWith10Plus();

    /// <summary>
    /// Retrieves a list of unique patient names for a given doctor,
    /// sorted alphabetically by full name.
    /// </summary>
    /// <param name="doctorId">The identifier of the doctor.</param>
    /// <returns>A collection of distinct patient full names.</returns>
    public IEnumerable<string> GetPatientNamesForDoctor(int doctorId);

    /// <summary>
    /// Counts the number of follow-up appointments within the last month
    /// relative to the specified date.
    /// </summary>
    /// <param name="now">The current date and time reference.</param>
    /// <returns>The number of follow-up appointments in the last month.</returns>
    public int GetFollowUpsLastMonth(DateTime now);

    /// <summary>
    /// Retrieves patients older than 30 years who have visited at least
    /// two different doctors, sorted by birth date.
    /// </summary>
    /// <param name="today">The reference date used to calculate age.</param>
    /// <returns>A collection of <see cref="PatientDto"/> objects.</returns>
    public IEnumerable<PatientDto> GetPatients30PlusMultiDoctors(DateOnly today);

    /// <summary>
    /// Retrieves appointments scheduled in the specified room for the current month,
    /// ordered by start time.
    /// </summary>
    /// <param name="room">The room number or name.</param>
    /// <param name="now">The current date and time reference.</param>
    /// <returns>A collection of <see cref="AppointmentDto"/> objects.</returns>
    public IEnumerable<AppointmentDto> GetThisMonthInRoom(string room, DateTime now);
}
