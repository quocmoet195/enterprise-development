using Hospital.Application.Contracts.Appointments;
using Hospital.Application.Contracts.Doctors;
using Hospital.Application.Contracts.Patients;

namespace Hospital.Web;

public class HospitalApiClient(HttpClient httpClient)
{
    public async Task<List<AppointmentDto>> GetAppointmentsAsync(CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<List<AppointmentDto>>("/api/appointments", cancellationToken) ?? [];
    }

    public async Task<List<DoctorDto>> GetDoctorsAsync(CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<List<DoctorDto>>("/api/doctors", cancellationToken) ?? [];
    }

    public async Task<List<PatientDto>> GetPatientsAsync(CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<List<PatientDto>>("/api/patients", cancellationToken) ?? [];
    }

    public async Task<List<DoctorDto>> GetExperiencedDoctorsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<DoctorDto>>("/api/analytics/doctors/10plus") ?? [];
    }

    public async Task<List<string>> GetPatientNamesByDoctorAsync(int doctorId)
    {
        return await httpClient.GetFromJsonAsync<List<string>>($"/api/analytics/doctor/{doctorId}/patients") ?? [];
    }

    public async Task<int> GetFollowUpsCountAsync()
    {
        return await httpClient.GetFromJsonAsync<int>("/api/analytics/followups/last-month");
    }

    public async Task<List<PatientDto>> GetComplexPatientsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<PatientDto>>("/api/analytics/patients/30plus-multi-doctors") ?? [];
    }

    public async Task<List<AppointmentDto>> GetRoomScheduleAsync(string room)
    {
        return await httpClient.GetFromJsonAsync<List<AppointmentDto>>($"/api/analytics/appointments/this-month?room={room}") ?? [];
    }


}