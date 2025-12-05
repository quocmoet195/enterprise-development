namespace Hospital.Infrastructure.Nats;

public class HospitalNatsOptions
{
    public string SubjectDoctors { get; set; } = "hospital.doctors";
    public string SubjectPatients { get; set; } = "hospital.patients";
    public string SubjectAppointments { get; set; } = "hospital.appointments";
}
