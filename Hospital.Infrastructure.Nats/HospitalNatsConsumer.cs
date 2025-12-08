using Microsoft.Extensions.Hosting;
using System.Text;
using System.Text.Json;
using NATS.Client.Core;
using Microsoft.Extensions.Logging;

namespace Hospital.Infrastructure.Nats;

public class HospitalNatsConsumer(INatsConnection client) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("NATS Consumer is running...");

        await foreach (var msg in client.SubscribeAsync<byte[]>(
            "hospital.appointments", cancellationToken: stoppingToken))
        {
            if (msg.Data == null)
                continue;

            var json = Encoding.UTF8.GetString(msg.Data);
            var apps = JsonSerializer.Deserialize<List<AppointmentMessage>>(json);

            Console.WriteLine($"📩 Received contracts = {apps?.Count}");
        }
    }
}

public class AppointmentMessage
{
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public DateTime Time { get; set; }
}
