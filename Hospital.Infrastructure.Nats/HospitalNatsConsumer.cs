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
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("NATS Consumer is running...");
        var subject = options.Value.SubjectAppointments;

        try
        {
            await foreach (var msg in client.SubscribeAsync<byte[]>(subject, cancellationToken: stoppingToken))
            {
                if (msg.Data is null || msg.Data.Length == 0) continue;

                try
                {
                    var appsDto = JsonSerializer.Deserialize<List<AppointmentMessage>>(msg.Data);

                    if (appsDto is not null && appsDto.Count > 0)
                    {
                        await SaveToDatabaseAsync(appsDto, stoppingToken);

                        logger.LogInformation("Saved {Count} appointments to DB", appsDto.Count);
                    }
                }
                catch (JsonException ex)
                {
                    logger.LogError(ex, "JSON Error");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing message");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "NATS connection failed");
        }
    }

    private async Task SaveToDatabaseAsync(List<AppointmentMessage> messages, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HospitalDbContext>();

        var entities = messages.Select(m => new Appointment
        {
            DoctorId = m.DoctorId,
            PatientId = m.PatientId,
            StartAt = m.Time,
            RoomNumber = "TBD" 
        });

        await dbContext.Appointments.AddRangeAsync(entities, ct);
        await dbContext.SaveChangesAsync(ct);
    }
}