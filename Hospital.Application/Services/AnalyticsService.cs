using AutoMapper;
using Hospital.Application.Contracts;
using Hospital.Application.Contracts.Appointments;
using Hospital.Application.Contracts.Doctors;
using Hospital.Application.Contracts.Patients;
using Hospital.Domain.Interfaces;

namespace Hospital.Application.Services;

/// <summary>
/// Provides analytics over doctors, patients, and appointments using repository abstractions.
/// </summary>
public class AnalyticsService(
    IDoctorRepository doctors,
    IAppointmentRepository appointments,
    IMapper mapper) : IAnalyticsService
{
    public IEnumerable<DoctorDto> GetDoctorsWith10Plus() =>
        doctors.GetAll()
            .Where(d => d.ExperienceYears >= 10)
            .Select(mapper.Map<DoctorDto>);

    public IEnumerable<string> GetPatientNamesForDoctor(int doctorId) =>
        appointments.GetAll()
            .Where(a => a.Doctor?.Id == doctorId)
            .Select(a => a.Patient?.FullName)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Distinct()
            .Order()!;

    public int GetFollowUpsLastMonth(DateTime now)
    {
        var from = now.AddMonths(-1);
        return appointments.GetAll()
            .Count(a => a.IsFollowUp && a.StartAt >= from && a.StartAt <= now);
    }

    public IEnumerable<PatientDto> GetPatients30PlusMultiDoctors(DateOnly today)
    {
        static int Age(DateOnly b, DateOnly t)
        {
            var age = t.Year - b.Year;
            if (b > t.AddYears(-age)) age--;
            return age;
        }

        var query = appointments.GetAll()
                        .Where(a => a.Patient != null && a.Doctor != null)
                        .GroupBy(a => a.Patient!)
                        .Where(g => Age(g.Key.BirthDate, today) > 30 && g.Select(x => x.Doctor!.Id).Distinct().Count() >= 2)
                        .Select(g => g.Key)
                        .OrderBy(p => p.BirthDate);

        return query.Select(mapper.Map<PatientDto>);
    }

    public IEnumerable<AppointmentDto> GetThisMonthInRoom(string room, DateTime now)
    {
        var start = new DateTime(now.Year, now.Month, 1);
        var end = start.AddMonths(1);

        var query = appointments.GetAll()
                        .Where(a => a.RoomNumber == room && a.StartAt >= start && a.StartAt < end)
                        .OrderBy(a => a.StartAt);

        return query.Select(mapper.Map<AppointmentDto>);
    }
}
