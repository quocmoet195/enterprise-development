using AutoMapper;
using Hospital.Application.Contracts.Doctors;
using Hospital.Application.Contracts.Patients;
using Hospital.Application.Contracts.Appointments;
using Hospital.Domain.Entities;

namespace Hospital.Application;

/// <summary>
/// Defines mapping configurations between domain entities and DTOs
/// for the Hospital application.
/// </summary>
public class HospitalProfile : Profile
{
    /// <summary>
    /// Initializes the <see cref="HospitalProfile"/> and defines all entity-to-DTO mappings.
    /// </summary>
    public HospitalProfile()
    {
        // Maps specialization enum to string.
        CreateMap<Doctor, DoctorDto>()
            .ForCtorParam(
                nameof(DoctorDto.Specialization),
                opt => opt.MapFrom(src => src.Specialization.ToString())
            );

        // Maps enum values (Gender, BloodGroup, Rhesus) to readable strings.
        CreateMap<Patient, PatientDto>()
            .ForCtorParam(
                nameof(PatientDto.Gender),
                opt => opt.MapFrom(src => src.Gender.ToString())
            )
            .ForCtorParam(
                nameof(PatientDto.BloodGroup),
                opt => opt.MapFrom(src => src.BloodGroup.ToString())
            )
            .ForCtorParam(
                nameof(PatientDto.Rhesus),
                opt => opt.MapFrom(src => src.Rhesus.ToString())
            );

        // Includes doctor/patient names and IDs for display in API results.
        CreateMap<Appointment, AppointmentDto>()
            .ForCtorParam(
                nameof(AppointmentDto.DoctorId),
                opt => opt.MapFrom(src => src.Doctor!.Id)
            )
            .ForCtorParam(
                nameof(AppointmentDto.DoctorName),
                opt => opt.MapFrom(src => src.Doctor!.FullName)
            )
            .ForCtorParam(
                nameof(AppointmentDto.PatientId),
                opt => opt.MapFrom(src => src.Patient!.Id)
            )
            .ForCtorParam(
                nameof(AppointmentDto.PatientName),
                opt => opt.MapFrom(src => src.Patient!.FullName)
            );
    }
}
