using Hospital.Domain.Entities;

namespace Hospital.Domain.Interfaces;

/// <summary>
/// Repository interface for Doctor aggregate.
/// </summary>
public interface IDoctorRepository : IRepository<Doctor>
{
    // Add doctor-specific methods here if needed (e.g., FindByPassport)
}
