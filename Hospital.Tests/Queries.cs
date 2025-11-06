using Hospital.Domain.Entities;

namespace Hospital.Tests;

/// <summary>
/// Provides LINQ queries for doctors, patients, and appointments.
/// </summary>
public static class Queries
{
    public static List<Doctor> DoctorsWith10Plus(List<Doctor> doctors) =>
        [.. doctors.Where(d => d.ExperienceYears >= 10)];


    public static List<string> PatientNamesForDoctor(List<Appointment> apps, int doctorId) =>
        [.. apps.Where(a => a.Doctor != null && a.Doctor!.Id == doctorId)
                .Select(a => a.Patient)
                .Where(p => p is not null)
                .Select(p => p!.FullName)
                .Distinct()
                .OrderBy(n => n)];


    public static int FollowUpsLastMonth(List<Appointment> apps, DateTime now)
    {
        var from = now.AddMonths(-1);
        return apps.Count(a => a.IsFollowUp && a.StartAt >= from && a.StartAt <= now);
    }

    public static List<Patient> Patients30PlusMultiDoctors(List<Appointment> apps, DateOnly today) =>
        [.. apps.Where(a => a.Patient != null)
                .GroupBy(a => a.Patient!)
                .Where(g => GetAge(g.Key.BirthDate, today) > 30 &&
                        g.Select(x => x.Doctor?.Id).Distinct().Count() >= 2)
                .Select(g => g.Key)
                .OrderBy(p => p.BirthDate)];

    public static List<Appointment> ThisMonthInRoom(List<Appointment> apps, string room, DateTime now)
    {
        var start = new DateTime(now.Year, now.Month, 1);
        var end = start.AddMonths(1);

        return [.. apps.Where(a => a.RoomNumber == room && a.StartAt >= start && a.StartAt < end)
                       .OrderBy(a => a.StartAt)];
    }

    private static int GetAge(DateOnly birth, DateOnly today)
    {
        var age = today.Year - birth.Year;
        if (birth > today.AddYears(-age)) age--;
        return age;
    }
}
