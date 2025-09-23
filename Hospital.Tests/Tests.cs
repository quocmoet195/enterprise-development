using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Domain.Entities;
using Xunit;

namespace Hospital.Tests;

/// <summary>
/// Container for query methods and their unit tests.
/// </summary>
public class Tests
{
    /// <summary>
    /// Provides LINQ queries for doctors, patients, and appointments.
    /// </summary>
    public static class Queries
    {
        /// <summary>
        /// Returns all doctors with at least 10 years of experience.
        /// </summary>
        /// <param name="doctors">List of doctors to search.</param>
        /// <returns>List of doctors meeting the criteria.</returns>
        public static List<Doctor> DoctorsWith10Plus(List<Doctor> doctors) =>
            doctors.Where(d => d.ExperienceYears >= 10).ToList();

        /// <summary>
        /// Returns distinct patients for a specific doctor, sorted by full name.
        /// </summary>
        /// <param name="apps">Appointments to search.</param>
        /// <param name="doctorId">Doctor identifier.</param>
        /// <returns>Sorted list of unique patients.</returns>
        public static List<Patient> PatientsForDoctor(List<Appointment> apps, int doctorId) =>
            apps.Where(a => a.DoctorId == doctorId)
                .Select(a => a.Patient)
                .Where(p => p != null)
                .DistinctBy(p => p!.Id)
                .OrderBy(p => p!.FullName)
                .ToList()!;

        /// <summary>
        /// Counts follow-up appointments in the last month, relative to <paramref name="now"/>.
        /// </summary>
        /// <param name="apps">Appointments to search.</param>
        /// <param name="now">Reference date for calculating the last month.</param>
        /// <returns>Number of follow-up appointments in the last month.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="now"/> is too small to subtract one month.
        /// </exception>
        public static int FollowUpsLastMonth(List<Appointment> apps, DateTime now)
        {
            if (now <= DateTime.MinValue.AddMonths(1))
                throw new ArgumentOutOfRangeException(nameof(now), "Parameter 'now' is too small for AddMonths(-1).");

            var from = now.AddMonths(-1);
            return apps.Count(a => a.IsFollowUp && a.StartAt >= from && a.StartAt <= now);
        }

        /// <summary>
        /// Returns patients older than 30 who have appointments with at least 2 distinct doctors,
        /// sorted by birth date.
        /// </summary>
        /// <param name="apps">Appointments to search.</param>
        /// <param name="today">Current date for age calculation.</param>
        /// <returns>Sorted list of patients meeting the criteria.</returns>
        public static List<Patient> Patients30PlusMultiDoctors(List<Appointment> apps, DateOnly today) =>
            apps.Where(a => a.Patient != null)
                .GroupBy(a => a.Patient!)
                .Where(g => GetAge(g.Key.BirthDate, today) > 30 &&
                            g.Select(x => x.DoctorId).Distinct().Count() >= 2)
                .Select(g => g.Key)
                .OrderBy(p => p.BirthDate)
                .ToList();

        /// <summary>
        /// Returns all appointments for a specific room in the current month,
        /// sorted by start time.
        /// </summary>
        /// <param name="apps">Appointments to search.</param>
        /// <param name="room">Room number to filter by.</param>
        /// <param name="now">Current date to determine the month.</param>
        /// <returns>Sorted list of appointments in the room for the current month.</returns>
        public static List<Appointment> ThisMonthInRoom(List<Appointment> apps, string room, DateTime now)
        {
            var start = new DateTime(now.Year, now.Month, 1);
            var end = start.AddMonths(1);

            return apps.Where(a => a.RoomNumber == room &&
                                   a.StartAt >= start &&
                                   a.StartAt < end)
                       .OrderBy(a => a.StartAt)
                       .ToList();
        }

        /// <summary>
        /// Calculates the age of a patient.
        /// </summary>
        /// <param name="birth">Birth date.</param>
        /// <param name="today">Current date.</param>
        /// <returns>Age in full years.</returns>
        private static int GetAge(DateOnly birth, DateOnly today)
        {
            var age = today.Year - birth.Year;
            if (birth > today.AddYears(-age)) age--;
            return age;
        }
    }

    /// <summary>
    /// Unit tests.
    /// </summary>
    public class QueriesTests
    {
        private readonly List<Doctor> _doctors = TestData.BuildDoctors();
        private readonly List<Patient> _patients = TestData.BuildPatients();
        private readonly List<Appointment> _apps;

        private static readonly DateTime _now = TestData.Now;
        private static readonly DateOnly _today = TestData.Today;

        public QueriesTests()
        {
            _apps = TestData.BuildAppointments(_doctors, _patients);
        }

        /// <summary>
        /// Verifies that only doctors with >= 10 years experience are returned.
        /// </summary>
        [Fact]
        public void DoctorsWith10Plus_Returns_Only_Those_With_Experience_AtLeast_10()
        {
            var result = Tests.Queries.DoctorsWith10Plus(_doctors);
            Assert.NotEmpty(result);
            Assert.All(result, d => Assert.True(d.ExperienceYears >= 10));
            Assert.DoesNotContain(result, d => d.ExperienceYears < 10);
        }

        /// <summary>
        /// Verifies that patients for a doctor are distinct and sorted by full name.
        /// </summary>
        [Fact]
        public void PatientsForDoctor_Returns_Distinct_Patients_Sorted_By_Name()
        {
            var doctorId = 1;
            var result = Tests.Queries.PatientsForDoctor(_apps, doctorId);

            var names = result.Select(p => p.FullName).ToList();
            Assert.Equal(new[] { "Podtyagina Anastasia", "Yemets Timofey" }, names);

            Assert.Equal(result.Count, result.Select(p => p.Id).Distinct().Count());
        }

        /// <summary>
        /// Verifies that only follow-ups in the last month are counted.
        /// </summary>
        [Fact]
        public void FollowUpsLastMonth_Counts_Only_FollowUps_In_Window()
        {
            var count = Tests.Queries.FollowUpsLastMonth(_apps, _now);
            Assert.Equal(2, count);
        }

        /// <summary>
        /// Verifies that patients older than 30 with 2+ doctors are returned,
        /// and that they are sorted by birth date.
        /// </summary>
        [Fact]
        public void Patients30PlusMultiDoctors_Filters_And_Orders_By_BirthDate()
        {
            var result = Tests.Queries.Patients30PlusMultiDoctors(_apps, _today);

            var ids = result.Select(p => p.Id).ToList();
            Assert.DoesNotContain(3, ids);
            Assert.Contains(1, ids);
            Assert.Contains(2, ids);

            var names = result.Select(p => p.FullName).ToList();
            Assert.Equal(new[] { "Podtyagina Anastasia", "Yemets Timofey" }, names);
        }

        /// <summary>
        /// Verifies that appointments are correctly filtered by room and month,
        /// and sorted by start time.
        /// </summary>
        [Fact]
        public void ThisMonthInRoom_Filters_By_Room_And_Current_Month_And_Sorts_By_Start()
        {
            var room = "301";
            var result = Tests.Queries.ThisMonthInRoom(_apps, room, _now);

            Assert.Equal(3, result.Count);
            Assert.All(result, a => Assert.Equal(room, a.RoomNumber));

            var times = result.Select(a => a.StartAt).ToList();
            Assert.True(times.SequenceEqual(times.OrderBy(t => t)));
        }
    }
}
