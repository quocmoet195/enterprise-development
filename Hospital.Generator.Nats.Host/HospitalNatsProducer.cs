using Bogus;
using System.Text.Json;
using Hospital.Application.Contracts.Doctors;
using Hospital.Application.Contracts.Patients;
using Hospital.Application.Contracts.Appointments;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NATS.Client.Core;
using Hospital.Domain.Enums;
using System.Linq;

namespace Hospital.Generator.Nats.Host;

public class HospitalNatsProducer(
    ILogger<HospitalNatsProducer> logger, 
    IOptions<NatsOptions> options,
    INatsConnection nats) 
    : BackgroundService
{
    private readonly NatsOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("HospitalNatsProducer started");
        var faker = new Faker("ru");
        var rnd = Random.Shared;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var batchSize = _options.BatchSize;

                var doctors = new List<DoctorCreateUpdateDto>(Enumerable.Range(1, batchSize)
                    .Select(i => new DoctorCreateUpdateDto(
                        FullName: faker.Name.FullName(),
                        BirthYear: 1970 + rnd.Next(0, 25),
                        ExperienceYears: rnd.Next(1, 40),
                        Specialization: faker.PickRandom<DoctorSpecialization>().ToString()))

                    );

                await PublishAsync(nats, _options.SubjectDoctors, doctors, stoppingToken);

                var patients = new List<PatientCreateUpdateDto>(Enumerable.Range(1, batchSize)
                    .Select(i => new PatientCreateUpdateDto(
                        Passport: faker.Random.Replace("##??######"), 
                        FullName: faker.Name.FullName(),
                        Gender: faker.PickRandom<Gender>(),
                        BirthDate: DateOnly.FromDateTime(faker.Date.Past(50, DateTime.UtcNow.AddYears(-18))), 
                        Address: faker.Address.StreetAddress(), 
                        BloodGroup: faker.PickRandom<BloodGroup>(),
                        Rhesus: faker.PickRandom<RhesusFactor>(),
                        Phone: faker.Phone.PhoneNumber() 
                    )));

                await PublishAsync(nats, _options.SubjectPatients, patients, stoppingToken);

                var now = DateTime.UtcNow;

                var appointments = new List<AppointmentCreateUpdateDto>(Enumerable.Range(1, batchSize)
                    .Select(i => new AppointmentCreateUpdateDto(
                        StartAt: now.AddDays(i),
                        RoomNumber: faker.Random.Int(100, 500).ToString(),
                        IsFollowUp: i % 2 == 0,
                        DoctorId: i,
                        PatientId: i))
                    );

                await PublishAsync(nats, _options.SubjectAppointments, appointments, stoppingToken);

                logger.LogInformation(
                    "Sent batch of {Count} doctors, {Count} patients, {Count} appointments",
                    doctors.Count, patients.Count, appointments.Count);

                await Task.Delay(TimeSpan.FromSeconds(_options.DelaySeconds), stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while publishing data to NATS");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    private static async Task PublishAsync<T>(
        INatsConnection nats,
        string subject,
        T payload,
        CancellationToken ct)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(payload);
        await nats.PublishAsync(subject, bytes, cancellationToken: ct);
    }
}
