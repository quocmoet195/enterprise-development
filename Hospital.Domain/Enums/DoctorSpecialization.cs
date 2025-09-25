namespace Hospital.Domain.Enums;

/// <summary>
/// Medical specializations available for doctors.
/// </summary>
public enum DoctorSpecialization
{
    /// <summary>
    /// General therapist (internal medicine).
    /// </summary>
    Therapist,

    /// <summary>
    /// Surgeon.
    /// </summary>
    Surgeon,

    /// <summary>
    /// Cardiologist (heart and cardiovascular system).
    /// </summary>
    Cardiologist,

    /// <summary>
    /// Neurologist (nervous system).
    /// </summary>
    Neurologist,

    /// <summary>
    /// Pediatrician (children's doctor).
    /// </summary>
    Pediatrician,

    /// <summary>
    /// Dentist.
    /// </summary>
    Dentist,

    /// <summary>
    /// Other specialization not listed above.
    /// </summary>
    Other
}
