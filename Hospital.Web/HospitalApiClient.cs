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

    public async Task<List<PatientDto>> GetComplexPatientsAsync(DateTime? date = null)
    {
        var queryString = date.HasValue
            ? $"?today={date.Value:yyyy-MM-dd}"
            : "";

        return await httpClient.GetFromJsonAsync<List<PatientDto>>($"/api/analytics/patients/30plus-multi-doctors{queryString}") ?? [];
    }

    public async Task<List<AppointmentDto>> GetRoomScheduleAsync(string room)
    {
        return await httpClient.GetFromJsonAsync<List<AppointmentDto>>($"/api/analytics/appointments/this-month?room={room}") ?? [];
    }

    public async Task<bool> CreateDoctorAsync(DoctorCreateUpdateDto dto)
    {
        var response = await httpClient.PostAsJsonAsync("/api/doctors", dto);
        return response.IsSuccessStatusCode;
    }

    // 2. Cập nhật (PUT)
    public async Task<bool> UpdateDoctorAsync(int id, DoctorCreateUpdateDto dto)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/doctors/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    // 3. Xóa (DELETE)
    public async Task<bool> DeleteDoctorAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"/api/doctors/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreateAppointmentAsync(AppointmentCreateUpdateDto dto)
    {
        var response = await httpClient.PostAsJsonAsync("/api/appointments", dto);
        return response.IsSuccessStatusCode;
    }

    // 2. Cập nhật (PUT)
    public async Task<bool> UpdateAppointmentAsync(int id, AppointmentCreateUpdateDto dto)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/appointments/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    // 3. Xóa (DELETE)
    public async Task<bool> DeleteAppointmentAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"/api/appointments/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreatePatientAsync(PatientCreateUpdateDto dto)
    {
        var response = await httpClient.PostAsJsonAsync("/api/patients", dto);
        return response.IsSuccessStatusCode;
    }

    // 2. Cập nhật (PUT)
    public async Task<bool> UpdatePatientAsync(int id, PatientCreateUpdateDto dto)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/patients/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    // 3. Xóa (DELETE)
    public async Task<bool> DeletePatientAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"/api/patients/{id}");
        return response.IsSuccessStatusCode;
    }
}