namespace Hospital.Infrastructure.Nats;

public class DoctorMessage
{
    public string FullName { get; set; } = default!;
    public string Specialization { get; set; } = default!;
    public int BirthYear { get; set; }
    public int ExperienceYears { get; set; }
}