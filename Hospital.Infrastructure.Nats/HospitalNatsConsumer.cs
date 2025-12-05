using Microsoft.Extensions.Hosting;
using System.Text;
using System.Text.Json;
using NATS.Client.Core;

namespace Hospital.Infrastructure.Nats;

public class HospitalNatsConsumer : BackgroundService
{
    private readonly INatsConnection _client;

    public HospitalNatsConsumer(INatsConnection client)
    {
        _client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("NATS Consumer is running...");

        await foreach (var msg in _client.SubscribeAsync<byte[]>(
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
