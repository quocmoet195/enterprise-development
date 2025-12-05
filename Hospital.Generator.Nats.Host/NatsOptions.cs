namespace Hospital.Generator.Nats.Host;

public class NatsOptions
{
    public string Url { get; set; } = "nats://localhost:4222";
    public string SubjectDoctors { get; set; } = "hospital.doctors";
    public string SubjectPatients { get; set; } = "hospital.patients";
    public string SubjectAppointments { get; set; } = "hospital.appointments";
    public int BatchSize { get; set; } = 5;
    public int DelaySeconds { get; set; } = 5;
}
