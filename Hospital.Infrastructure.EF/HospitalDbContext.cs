using Microsoft.EntityFrameworkCore;
using Hospital.Domain.Entities;
using Hospital.Domain.Enums;

namespace Hospital.Infrastructure.EF;

public class HospitalDbContext : DbContext
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options) { }

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder b)
    {
        b.Entity<Doctor>().HasData(
            Enumerable.Range(1, 10).Select(i => new Doctor
            {
                Id = i,
                Passport = $"D{i:000000}",
                FullName = $"Doctor {i}",
                BirthYear = 1970 + (i % 20),
                Specialization = DoctorSpecialization.Therapist,
                ExperienceYears = i  + 5
            }).ToArray()
        );

        b.Entity<Patient>().HasData(
            new() { Id = 1, Passport = "P000001", FullName = "Podtyagina Anastasia", Gender = Gender.Female, BirthDate = new(1985, 1, 1), Address = "Addr1", BloodGroup = BloodGroup.A, Rhesus = RhesusFactor.Positive, Phone = "111" },
            new() { Id = 2, Passport = "P000002", FullName = "Yemets Timofey", Gender = Gender.Male, BirthDate = new(1990, 2, 2), Address = "Addr2", BloodGroup = BloodGroup.O, Rhesus = RhesusFactor.Negative, Phone = "222" },
            new() { Id = 3, Passport = "P000003", FullName = "Astsatryan Liliya", Gender = Gender.Male, BirthDate = new(2000, 3, 3), Address = "Addr3", BloodGroup = BloodGroup.B, Rhesus = RhesusFactor.Positive, Phone = "333" },
            new() { Id = 4, Passport = "P000004", FullName = "Ryzhova Alena", Gender = Gender.Female, BirthDate = new(1988, 4, 4), Address = "Addr4", BloodGroup = BloodGroup.Ab, Rhesus = RhesusFactor.Positive, Phone = "444" },
            new() { Id = 5, Passport = "P000005", FullName = "Ivanov Daniil", Gender = Gender.Male, BirthDate = new(1992, 5, 5), Address = "Addr5", BloodGroup = BloodGroup.A, Rhesus = RhesusFactor.Negative, Phone = "555" },
            new() { Id = 6, Passport = "P000006", FullName = "Dick Roman", Gender = Gender.Male, BirthDate = new(1981, 6, 6), Address = "Addr6", BloodGroup = BloodGroup.O, Rhesus = RhesusFactor.Positive, Phone = "666" },
            new() { Id = 7, Passport = "P000007", FullName = "Volkov Alexander", Gender = Gender.Male, BirthDate = new(1979, 7, 7), Address = "Addr7", BloodGroup = BloodGroup.B, Rhesus = RhesusFactor.Positive, Phone = "777" },
            new() { Id = 8, Passport = "P000008", FullName = "Klyushin Ivan", Gender = Gender.Male, BirthDate = new(1995, 8, 8), Address = "Addr8", BloodGroup = BloodGroup.Ab, Rhesus = RhesusFactor.Negative, Phone = "888" },
            new() { Id = 9, Passport = "P000009", FullName = "Semenova Alexandra", Gender = Gender.Female, BirthDate = new(1998, 9, 9), Address = "Addr9", BloodGroup = BloodGroup.A, Rhesus = RhesusFactor.Positive, Phone = "999" },
            new() { Id = 10, Passport = "P000010", FullName = "Grishin Nikita", Gender = Gender.Male, BirthDate = new(1987, 10, 10), Address = "Addr10", BloodGroup = BloodGroup.O, Rhesus = RhesusFactor.Positive, Phone = "000" }
        );

        b.Entity<Appointment>().HasData(
            new() { Id = 1, StartAt = new(2025, 9, 2, 9, 0, 0), RoomNumber = "101", IsFollowUp = false, PatientId = 1, DoctorId = 1 },
            new() { Id = 2, StartAt = new(2025, 9, 3, 10, 0, 0), RoomNumber = "101", IsFollowUp = false, PatientId = 2, DoctorId = 1 },
            new() { Id = 3, StartAt = new(2025, 9, 10, 11, 0, 0), RoomNumber = "101", IsFollowUp = false, PatientId = 1, DoctorId = 1 },

            new() { Id = 4, StartAt = new(2025, 8, 20, 12, 0, 0), RoomNumber = "102", IsFollowUp = true, PatientId = 3, DoctorId = 2 },
            new() { Id = 5, StartAt = new(2025, 9, 1, 13, 0, 0), RoomNumber = "103", IsFollowUp = true, PatientId = 4, DoctorId = 3 },
            new() { Id = 6, StartAt = new(2025, 7, 10, 14, 0, 0), RoomNumber = "104", IsFollowUp = true, PatientId = 5, DoctorId = 4 },
            new() { Id = 7, StartAt = new(2025, 9, 16, 15, 0, 0), RoomNumber = "105", IsFollowUp = true, PatientId = 6, DoctorId = 5 },
            new() { Id = 8, StartAt = new(2025, 9, 5, 9, 0, 0), RoomNumber = "101", IsFollowUp = false, PatientId = 7, DoctorId = 6 },

            new() { Id = 9, StartAt = new(2025, 9, 6, 9, 30, 0), RoomNumber = "201", IsFollowUp = false, PatientId = 1, DoctorId = 2 },
            new() { Id = 10, StartAt = new(2025, 9, 7, 9, 30, 0), RoomNumber = "201", IsFollowUp = false, PatientId = 1, DoctorId = 3 },
            new() { Id = 11, StartAt = new(2025, 9, 8, 9, 30, 0), RoomNumber = "202", IsFollowUp = false, PatientId = 2, DoctorId = 2 },
            new() { Id = 12, StartAt = new(2025, 9, 9, 9, 30, 0), RoomNumber = "202", IsFollowUp = false, PatientId = 2, DoctorId = 4 },
            new() { Id = 13, StartAt = new(2025, 9, 10, 8, 0, 0), RoomNumber = "203", IsFollowUp = false, PatientId = 3, DoctorId = 2 },
            new() { Id = 14, StartAt = new(2025, 9, 11, 8, 0, 0), RoomNumber = "203", IsFollowUp = false, PatientId = 3, DoctorId = 4 },

            new() { Id = 15, StartAt = new(2025, 9, 1, 8, 0, 0), RoomNumber = "301", IsFollowUp = false, PatientId = 8, DoctorId = 7 },
            new() { Id = 16, StartAt = new(2025, 9, 15, 10, 0, 0), RoomNumber = "301", IsFollowUp = false, PatientId = 9, DoctorId = 7 },
            new() { Id = 17, StartAt = new(2025, 9, 28, 13, 0, 0), RoomNumber = "301", IsFollowUp = false, PatientId = 10, DoctorId = 7 },
            new() { Id = 18, StartAt = new(2025, 8, 31, 8, 0, 0), RoomNumber = "301", IsFollowUp = false, PatientId = 8, DoctorId = 7 },
            new() { Id = 19, StartAt = new(2025, 10, 1, 8, 0, 0), RoomNumber = "301", IsFollowUp = false, PatientId = 9, DoctorId = 8 },
            new() { Id = 20, StartAt = new(2025, 9, 20, 8, 0, 0), RoomNumber = "302", IsFollowUp = false, PatientId = 10, DoctorId = 7 }
        );
    }
}
