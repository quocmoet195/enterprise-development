using AutoMapper;
using Hospital.Application.Contracts.Doctors;
using Hospital.Application.Contracts.Patients;
using Hospital.Application.Contracts.Appointments;
using Hospital.Domain.Entities;

namespace Hospital.Application;

/// <summary>
/// Defines AutoMapper configuration for the Hospital application.
/// Maps between domain entities (<see cref="Doctor"/>, <see cref="Patient"/>, <see cref="Appointment"/>) 
/// and their corresponding Data Transfer Objects (DTOs).
/// </summary>
public class HospitalProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HospitalProfile"/> class.
    /// Configures mapping rules for doctors, patients, and appointments.
    /// </summary>
    public HospitalProfile()
    {
        // Doctor mappings
        CreateMap<DoctorCreateUpdateDto, Doctor>()
            .ForMember(d => d.Passport, opt => opt.Ignore()); 

        CreateMap<Doctor, DoctorDto>()
            .ForCtorParam(nameof(DoctorDto.Specialization),
                opt => opt.MapFrom(src => src.Specialization.ToString()));

        // Patient mappings
        CreateMap<PatientCreateUpdateDto, Patient>();

        CreateMap<Patient, PatientDto>()
            .ForCtorParam(nameof(PatientDto.Gender),
                opt => opt.MapFrom(src => src.Gender.ToString()))
            .ForCtorParam(nameof(PatientDto.BloodGroup),
                opt => opt.MapFrom(src => src.BloodGroup.ToString()))
            .ForCtorParam(nameof(PatientDto.Rhesus),
                opt => opt.MapFrom(src => src.Rhesus.ToString()));

        // Appointment mappings
        CreateMap<AppointmentCreateUpdateDto, Appointment>()
            .ForMember(a => a.Doctor, opt => opt.Ignore()) 
            .ForMember(a => a.Patient, opt => opt.Ignore());

        CreateMap<Appointment, AppointmentDto>()
            .ForCtorParam(nameof(AppointmentDto.DoctorId),
                opt => opt.MapFrom(src => src.Doctor!.Id))
            .ForCtorParam(nameof(AppointmentDto.DoctorName),
                opt => opt.MapFrom(src => src.Doctor!.FullName))
            .ForCtorParam(nameof(AppointmentDto.PatientId),
                opt => opt.MapFrom(src => src.Patient!.Id))
            .ForCtorParam(nameof(AppointmentDto.PatientName),
                opt => opt.MapFrom(src => src.Patient!.FullName))
            .ForCtorParam(nameof(AppointmentDto.StartAt),
                opt => opt.MapFrom(src => src.StartAt))
            .ForCtorParam(nameof(AppointmentDto.RoomNumber),
                opt => opt.MapFrom(src => src.RoomNumber))
            .ForCtorParam(nameof(AppointmentDto.IsFollowUp),
                opt => opt.MapFrom(src => src.IsFollowUp))
            .ForCtorParam(nameof(AppointmentDto.DoctorId),
                opt => opt.MapFrom(src => src.DoctorId))
            .ForCtorParam(nameof(AppointmentDto.DoctorName),
                opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.FullName : "Unknown"))
            .ForCtorParam(nameof(AppointmentDto.PatientId),
                opt => opt.MapFrom(src => src.PatientId))
            .ForCtorParam(nameof(AppointmentDto.PatientName),
                opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FullName : "Unknown"));
    }
}
