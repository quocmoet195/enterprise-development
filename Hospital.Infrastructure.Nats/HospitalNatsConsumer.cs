using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection; 
using NATS.Client.Core;
using Hospital.Infrastructure.EF; 
using Hospital.Domain.Entities;     

namespace Hospital.Infrastructure.Nats;

public class HospitalNatsConsumer(
    INatsConnection client,
    ILogger<HospitalNatsConsumer> logger,
    IOptions<HospitalNatsOptions> options,
    IServiceScopeFactory scopeFactory) 
    : BackgroundService
{
    private readonly HospitalNatsOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("NATS Consumer is running...");
        var t1 = ProcessDoctorsAsync(stoppingToken);
        var t2 = ProcessPatientsAsync(stoppingToken);
        var t3 = ProcessAppointmentsAsync(stoppingToken);

        await Task.WhenAll(t1, t2, t3);
    }
    private async Task ProcessDoctorsAsync(CancellationToken ct)
    {
        await ProcessMessageAsync<DoctorMessage>(
            _options.SubjectDoctors,
            ct,
            async (dbContext, messages) =>
            {
                var entities = messages.Select(m => new Doctor
                {
                    FullName = m.FullName,
                    Specialization = Domain.Enums.DoctorSpecialization.Therapist, 
                    BirthYear = m.BirthYear,
                    ExperienceYears = m.ExperienceYears,
                    Passport = Guid.NewGuid().ToString().Substring(0, 8) 
                });

                await dbContext.Doctors.AddRangeAsync(entities, ct);
            });
    }

    private async Task ProcessPatientsAsync(CancellationToken ct)
    {
        await ProcessMessageAsync<PatientMessage>(
            _options.SubjectPatients,
            ct,
            async (dbContext, messages) =>
            {
                var entities = messages.Select(m => new Patient
                {
                    FullName = m.FullName,
                    Passport = m.Passport,
                    Gender = m.Gender,
                    BirthDate = m.BirthDate,
                    Address = m.Address,
                    Phone = m.Phone,
                    BloodGroup = m.BloodGroup,
                    Rhesus = m.Rhesus
                });

                await dbContext.Patients.AddRangeAsync(entities, ct);
            });
    }

    private async Task ProcessAppointmentsAsync(CancellationToken ct)
    {
        await ProcessMessageAsync<AppointmentMessage>(
            _options.SubjectAppointments,
            ct,
            async (dbContext, messages) =>
            {
                var entities = messages.Select(m => new Appointment
                {
                    DoctorId = m.DoctorId,
                    PatientId = m.PatientId,
                    StartAt = m.Time,
                    RoomNumber = "101",
                    IsFollowUp = false
                });

                await dbContext.Appointments.AddRangeAsync(entities, ct);
            });
    }

    private async Task ProcessMessageAsync<T>(
        string subject,
        CancellationToken ct,
        Func<HospitalDbContext, List<T>, Task> saveAction)
    {
        try
        {
            await foreach (var msg in client.SubscribeAsync<byte[]>(subject, cancellationToken: ct))
            {
                if (msg.Data is null || msg.Data.Length == 0) continue;

                try
                {
                    var data = JsonSerializer.Deserialize<List<T>>(msg.Data);
                    if (data is not null && data.Count > 0)
                    {
                        using var scope = scopeFactory.CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<HospitalDbContext>();

                        await saveAction(dbContext, data);
                        await dbContext.SaveChangesAsync(ct);

                        logger.LogInformation("✅ Saved {Count} items from {Subject}", data.Count, subject);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "❌ Error processing message from {Subject}", subject);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "💀 Connection error on {Subject}", subject);
        }
    }
}