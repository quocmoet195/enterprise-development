namespace Hospital.Infrastructure.Nats;

public class AppointmentMessage
{
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public DateTime Time { get; set; }
}