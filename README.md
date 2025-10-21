# Поликлиника — Enterprise Development

## Описание
Учебная информационная система «Поликлиника» (вариант №77):  
- **БД:** MySQL  
- **Брокер:** NATS  
- **Цель:** пошаговая реализация сервисно-ориентированного приложения (классы → сервер → ORM → генератор данных → клиент, оркестрация Aspire).

## Состояние работ
- ✅ **ЛР1 — Классы и unit-тесты**
  - Доменные сущности: `Doctor`, `Patient`, `Appointment`
  - Перечисления: `Gender`, `BloodGroup`, `RhesusFactor`, `DoctorSpecialization`
  - LINQ-запросы (см. `Hospital.Tests/Queries.cs`)
  - Тесты xUnit (см. `Hospital.Tests/Tests.cs`)
    
- ✅ **ЛЛР2 — Сервер (REST API)**
  - Разработано серверное приложение **Hospital.Api.Host**
  - Реализованы CRUD-операции для сущностей:
    - `Doctor`, `Patient`, `Appointment`
  - Добавлены аналитические эндпоинты (из ЛР1):
    - `/api/analytics/doctors/10plus` — врачи с ≥10 годами стажа  
    - `/api/analytics/patients/doctor/{id}` — пациенты выбранного врача  
    - `/api/analytics/followups` — количество повторных приёмов за месяц  
    - `/api/analytics/patients/30plus` — пациенты старше 30 с 2+ врачами  
    - `/api/analytics/appointments/thismonth` — приёмы за текущий месяц  
  - Используется **хранение данных в памяти** (без БД)
  - Подключён **Swagger UI** для тестирования запросов  
  - Настроен **AutoMapper** для преобразования DTO ↔ Entities  
  - Реализована **DI (внедрение зависимостей)** в `Program.cs`


## Структура решения
```
Hospital.Domain
├── Entities
│   ├── Doctor.cs
│   ├── Patient.cs
│   └── Appointment.cs
└── Enums
    ├── Gender.cs
    ├── BloodGroup.cs
    ├── RhesusFactor.cs
    └── DoctorSpecialization.cs

Hospital.Application.Contracts
├── Appointments/
│   ├── AppointmentCreateUpdateDtoAppointmentDto.cs
│   ├── AppointmentDto.cs
|   └── IAppointmentService
├── Doctors/
│   ├── DoctorCreateUpdateDto.cs
│   ├── DoctorDto.cs
│   └── IDoctorService.cs
├── Patients/
│   ├── IPatientService.cs
│   ├── PatientCreateUpdateDto.cs
│   └── PatientDto.cs
└── IAnalyticsService.cs

Hospital.Application
├── Services/
│   ├── AnalyticsService.cs
│   ├── AppointmentService.cs
│   ├── DoctorService.cs
│   └── PatientService.cs
└── HospitalProfile.cs

Hospital.Infrastructure.InMemory
  ├── AppointmentInMemoryRepository.cs
  ├── DoctorInMemoryRepository.cs
  └── PatientInMemoryRepository.cs

Hospital.Api.Host
├── Controllers/
│   ├── AnalyticsController.cs
│   ├── AppointmentController.cs
│   ├── DoctorController.cs
│   └── PatientController.cs
└── Program.cs

Hospital.Tests
├── Queries.cs
├── TestData.cs
└── Tests.cs
```
