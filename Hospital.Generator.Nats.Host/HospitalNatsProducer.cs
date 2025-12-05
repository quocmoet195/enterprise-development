using System.Text.Json;
using Hospital.Application.Contracts.Doctors;
using Hospital.Application.Contracts.Patients;
using Hospital.Application.Contracts.Appointments;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NATS.Client.Core;
using Hospital.Domain.Enums;

namespace Hospital.Generator.Nats.Host;

public class HospitalNatsProducer : BackgroundService
{
    private readonly ILogger<HospitalNatsProducer> _logger;
    private readonly NatsOptions _options;

    public HospitalNatsProducer(
        ILogger<HospitalNatsProducer> logger,
        IOptions<NatsOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("HospitalNatsProducer started");

        var opts = NatsOpts.Default with
        {
            Url = _options.Url
        };

        await using var nats = new NatsConnection(opts);


        var rnd = new Random();

        while (!stoppingToken.IsCancellationRequested)
        {
            var batchSize = _options.BatchSize;

            var doctors = Enumerable.Range(1, batchSize)
                .Select(i => new DoctorCreateUpdateDto(
                    FullName: $"Doctor {i}",
                    BirthYear: 1970 + rnd.Next(0, 25),
                    ExperienceYears: rnd.Next(1, 30),
                    Specialization: "Therapist"))
                .ToList();

            await PublishAsync(nats, _options.SubjectDoctors, doctors, stoppingToken);

            var patients = Enumerable.Range(1, batchSize)
                .Select(i => new PatientCreateUpdateDto(
                    Passport:$"000{i}",
                    FullName: $"Patient {i}",
                    Gender: Gender.Male,
                    BirthDate: new DateOnly(1990, 1, 1).AddDays(rnd.Next(0, 3650)),
                    Address: $"Street {i}",
                    BloodGroup: BloodGroup.A,
                    Rhesus: RhesusFactor.Positive,
                    Phone: $"111-11{i:00}"
                ))
                .ToList();

            await PublishAsync(nats, _options.SubjectPatients, patients, stoppingToken);

            var now = DateTime.UtcNow;

            var appointments = Enumerable.Range(1, batchSize)
                .Select(i => new AppointmentCreateUpdateDto(
                    StartAt: now.AddDays(i),
                    RoomNumber: $"10{i}",
                    IsFollowUp: i % 2 == 0,
                    DoctorId: i,  
                    PatientId: i))
                .ToList();

            await PublishAsync(nats, _options.SubjectAppointments, appointments, stoppingToken);

            _logger.LogInformation(
                "Sent batch of {Count} doctors, {Count} patients, {Count} appointments",
                doctors.Count, patients.Count, appointments.Count);

            await Task.Delay(TimeSpan.FromSeconds(_options.DelaySeconds), stoppingToken);
        }
    }

    private static async Task PublishAsync<T>(
        NatsConnection nats,
        string subject,
        T payload,
        CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(payload);
        var bytes = System.Text.Encoding.UTF8.GetBytes(json);

        await nats.PublishAsync(subject, bytes, cancellationToken: ct); 
    }
}
