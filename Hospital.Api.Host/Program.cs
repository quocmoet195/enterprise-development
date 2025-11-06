using Hospital.Application;
using Hospital.Application.Contracts;
using Hospital.Application.Contracts.Appointments;
using Hospital.Application.Contracts.Doctors;
using Hospital.Application.Contracts.Patients;
using Hospital.Application.Services;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.EF;
using Hospital.Infrastructure.EF.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var cs = builder.Configuration.GetConnectionString("HospitalDb");
builder.Services.AddDbContext<HospitalDbContext>(opt =>
{
    opt.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(HospitalProfile));
builder.Services.AddDbContext<HospitalDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("HospitalDb"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("HospitalDb"))
    ));



builder.Services.AddScoped<IDoctorRepository, DoctorEfRepository>();
builder.Services.AddScoped<IPatientRepository, PatientEfRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentEfRepository>();

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
