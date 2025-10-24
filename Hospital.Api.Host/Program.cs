using Hospital.Application;                        
using Hospital.Application.Contracts.Appointments;
using Hospital.Application.Contracts.Doctors;
using Hospital.Application.Contracts.Patients;
using Hospital.Application.Contracts;
using Hospital.Application.Services;               
using Hospital.Domain.Interfaces;                 
using Hospital.Infrastructure.InMemory.Seed;
using Hospital.Infrastructure.InMemory;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hospital API", Version = "v1" });
});

builder.Services.AddAutoMapper(typeof(HospitalProfile));

builder.Services.AddSingleton<InMemoryData>();

builder.Services.AddScoped<IDoctorRepository, DoctorInMemoryRepository>();
builder.Services.AddScoped<IPatientRepository, PatientInMemoryRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentInMemoryRepository>();

builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
