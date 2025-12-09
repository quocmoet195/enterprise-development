using Hospital.Domain.Enums; 

namespace Hospital.Infrastructure.Nats;

public class PatientMessage
{
    public string Passport { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Address { get; set; } = default!;
    public Gender Gender { get; set; }
    public DateOnly BirthDate { get; set; }
    public BloodGroup BloodGroup { get; set; }
    public RhesusFactor Rhesus { get; set; }
}