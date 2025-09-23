using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Domain.Entities;

namespace Hospital.Tests;

/// <summary>
/// Provides deterministic seed data for unit tests. 
/// Ensures tests are reproducible by using fixed dates and predictable data.
/// </summary>
public static class TestData
{
    /// <summary>
    /// Fixed "now" reference date used in tests (22 September 2025).
    /// </summary>
    public static readonly DateTime Now = new(2025, 9, 22);

    /// <summary>
    /// Fixed "Today" reference date derived from "Now"/>.
    /// </summary>
    public static readonly DateOnly Today = DateOnly.FromDateTime(Now.Date);

    /// <summary>
    /// Builds a list of doctors with IDs, passports, names, birth years,
    /// specialization, and varying experience years.
    /// </summary>
    /// <returns>List of 10 doctors.</returns>
    public static List<Doctor> BuildDoctors()
        => Enumerable.Range(1, 10).Select(i => new Doctor
        {
            Id = i,
            Passport = $"D{i:000000}",
            FullName = $"Doctor {i}",
            BirthYear = 1970 + (i % 20),
            Specialization = "Терапевт",
            ExperienceYears = i + 5
        }).ToList();

    /// <summary>
    /// Builds a list of patients with IDs, passports, names, genders,
    /// and birthdates. Used for age and uniqueness tests.
    /// </summary>
    /// <returns>List of 10 patients.</returns>
    public static List<Patient> BuildPatients() => new()
    {
        new() { Id = 1,  Passport = "P000001", FullName = "Podtyagina Anastasia",  Gender = Gender.Female, BirthDate = new(1985, 1, 1) },
        new() { Id = 2,  Passport = "P000002", FullName = "Yemets Timofey",        Gender = Gender.Male,   BirthDate = new(1990, 2, 2) },
        new() { Id = 3,  Passport = "P000003", FullName = "Astsatryan Liliya",     Gender = Gender.Male,   BirthDate = new(2000, 3, 3) },
        new() { Id = 4,  Passport = "P000004", FullName = "Ryzhova Alena",         Gender = Gender.Female, BirthDate = new(1988, 4, 4) },
        new() { Id = 5,  Passport = "P000005", FullName = "Ivanov Daniil",         Gender = Gender.Male,   BirthDate = new(1992, 5, 5) },
        new() { Id = 6,  Passport = "P000006", FullName = "Dick Roman",            Gender = Gender.Male,   BirthDate = new(1981, 6, 6) },
        new() { Id = 7,  Passport = "P000007", FullName = "Volkov Alexander",      Gender = Gender.Male,   BirthDate = new(1979, 7, 7) },
        new() { Id = 8,  Passport = "P000008", FullName = "Klyushin Ivan",         Gender = Gender.Male,   BirthDate = new(1995, 8, 8) },
        new() { Id = 9,  Passport = "P000009", FullName = "Semenova Alexandra",    Gender = Gender.Female, BirthDate = new(1998, 9, 9) },
        new() { Id = 10, Passport = "P000010", FullName = "Grishin Nikita",        Gender = Gender.Male,   BirthDate = new(1987, 10, 10) },
    };

    /// <summary>
    /// Builds a deterministic set of appointments linking patients and doctors.
    /// </summary>
    /// <param name="doctors">List of doctors</param>
    /// <param name="patients">List of patients</param>
    /// <returns>List of appointments for test scenarios.</returns>
    public static List<Appointment> BuildAppointments(List<Doctor> doctors, List<Patient> patients)
    {
        var apps = new List<Appointment>();

        Doctor D(int id) => doctors.Single(d => d.Id == id);
        Patient P(int id) => patients.Single(p => p.Id == id);

        void Add(Patient p, Doctor d, DateTime at, string room, bool followUp = false)
        {
            apps.Add(new Appointment
            {
                Id = apps.Count + 1,
                StartAt = at,
                RoomNumber = room,
                IsFollowUp = followUp,
                Patient = p,
                PatientId = p.Id,
                Doctor = d,
                DoctorId = d.Id
            });
        }

        // PatientsForDoctor (doctor 1)
        Add(P(1), D(1), new(2025, 9, 02, 9, 0, 0), "101");
        Add(P(2), D(1), new(2025, 9, 03, 10, 0, 0), "101");
        Add(P(1), D(1), new(2025, 9, 10, 11, 0, 0), "101");

        // FollowUpsLastMonth window [2025-08-15..2025-09-15]
        Add(P(3), D(2), new(2025, 8, 20, 12, 0, 0), "102", true);
        Add(P(4), D(3), new(2025, 9, 01, 13, 0, 0), "103", true);
        Add(P(5), D(4), new(2025, 7, 10, 14, 0, 0), "104", true); 
        Add(P(6), D(5), new(2025, 9, 16, 15, 0, 0), "105", true); 
        Add(P(7), D(6), new(2025, 9, 05, 9, 0, 0), "101", false);

        // Patients30PlusMultiDoctors
        // Alice + Bob (Chris is <30)
        Add(P(1), D(2), new(2025, 9, 06, 9, 30, 0), "201");
        Add(P(1), D(3), new(2025, 9, 07, 9, 30, 0), "201");
        Add(P(2), D(2), new(2025, 9, 08, 9, 30, 0), "202");
        Add(P(2), D(4), new(2025, 9, 09, 9, 30, 0), "202");
        Add(P(3), D(2), new(2025, 9, 10, 8, 0, 0), "203");
        Add(P(3), D(4), new(2025, 9, 11, 8, 0, 0), "203");

        // ThisMonthInRoom (room 301, September)
        Add(P(8), D(7), new(2025, 9, 01, 8, 0, 0), "301");
        Add(P(9), D(7), new(2025, 9, 15, 10, 0, 0), "301");
        Add(P(10), D(7), new(2025, 9, 28, 13, 0, 0), "301");
        Add(P(8), D(7), new(2025, 8, 31, 8, 0, 0), "301"); 
        Add(P(9), D(8), new(2025, 10, 01, 8, 0, 0), "301"); 
        Add(P(10), D(7), new(2025, 9, 20, 8, 0, 0), "302"); 

        return apps;
    }
}
