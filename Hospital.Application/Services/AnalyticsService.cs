using AutoMapper;
using Hospital.Application.Contracts;
using Hospital.Application.Contracts.Appointments;
using Hospital.Application.Contracts.Doctors;
using Hospital.Application.Contracts.Patients;
using Hospital.Tests;
using Hospital.Domain.Entities;

namespace Hospital.Application.Services;

/// <summary>
/// Provides analytical operations for hospital data such as statistics about doctors, patients, and appointments.
/// </summary>
public class AnalyticsService(TestData seed, IMapper mapper) : IAnalyticsService
{
    /// <summary>
    /// Retrieves all doctors who have 10 or more years of experience.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="DoctorDto"/> objects representing experienced doctors.
    /// </returns>
    public IEnumerable<DoctorDto> GetDoctorsWith10Plus()
        => seed.Doctors
               .Where(d => d.ExperienceYears >= 10)
               .Select(mapper.Map<DoctorDto>);

    /// <summary>
    /// Returns a list of unique patient names treated by a specific doctor.
    /// </summary>
    /// <param name="doctorId">The ID of the doctor whose patients are being retrieved.</param>
    /// <returns>
    /// A sorted collection of patient full names.
    /// </returns>
    public IEnumerable<string> GetPatientNamesForDoctor(int doctorId)
        => seed.Appointments
              .Where(a => a.Doctor?.Id == doctorId)
              .Select(a => a.Patient?.FullName)
              .Where(n => !string.IsNullOrWhiteSpace(n))
              .Distinct()!
              .OrderBy(n => n)!;

    /// <summary>
    /// Calculates the number of follow-up appointments that occurred within the last month from the given date.
    /// </summary>
    /// <param name="now">The reference date for calculating the last month window.</param>
    /// <returns>The number of follow-up appointments.</returns>
    public int GetFollowUpsLastMonth(DateTime now)
    {
        var from = now.AddMonths(-1);
        return seed.Appointments.Count(a => a.IsFollowUp && a.StartAt >= from && a.StartAt <= now);
    }

    /// <summary>
    /// Calculates age in full years based on the birth date and the current date.
    /// </summary>
    /// <param name="b">Birth date of the person.</param>
    /// <param name="t">The date to calculate the age at.</param>
    /// <returns>The calculated age in years.</returns>
    private static int Age(DateOnly b, DateOnly t)
    {
        var age = t.Year - b.Year;
        if (b > t.AddYears(-age)) age--;
        return age;
    }

    /// <summary>
    /// Retrieves patients older than 30 years who have had appointments with at least two different doctors.
    /// </summary>
    /// <param name="today">The date used to calculate patients' ages.</param>
    /// <returns>
    /// A collection of <see cref="PatientDto"/> objects representing patients meeting the criteria.
    /// </returns>
    public IEnumerable<PatientDto> GetPatients30PlusMultiDoctors(DateOnly today)
    {
        var q = seed.Appointments
            .Where(a => a.Patient != null && a.Doctor != null)
            .GroupBy(a => a.Patient!)
            .Where(g => Age(g.Key.BirthDate, today) > 30 &&
                        g.Select(x => x.Doctor!.Id).Distinct().Count() >= 2)
            .Select(g => g.Key)
            .OrderBy(p => p.BirthDate);

        return q.Select(mapper.Map<PatientDto>);
    }

    /// <summary>
    /// Retrieves all appointments for a given room number that are scheduled within the current month.
    /// </summary>
    /// <param name="room">The room number to filter appointments by.</param>
    /// <param name="now">The current date used to determine the current month range.</param>
    /// <returns>
    /// A collection of <see cref="AppointmentDto"/> objects ordered by appointment start time.
    /// </returns>
    public IEnumerable<AppointmentDto> GetThisMonthInRoom(string room, DateTime now)
    {
        var start = new DateTime(now.Year, now.Month, 1);
        var end = start.AddMonths(1);
        var q = seed.Appointments
            .Where(a => a.RoomNumber == room && a.StartAt >= start && a.StartAt < end)
            .OrderBy(a => a.StartAt);

        return q.Select(mapper.Map<AppointmentDto>);
    }
}
