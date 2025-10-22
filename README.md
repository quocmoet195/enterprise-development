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
├── Doctors/
│   ├── DoctorDto.cs
│   └── DoctorCreateUpdateDto.cs
├── Patients/
│   ├── PatientDto.cs
│   └── PatientCreateUpdateDto.cs
├── Appointments/
│   ├── AppointmentDto.cs
│   └── AppointmentCreateUpdateDto.cs
└── IAnalyticsService.cs

Hospital.Application
├── Services/
│   ├── DoctorService.cs
│   ├── PatientService.cs
│   ├── AppointmentService.cs
│   └── AnalyticsService.cs
└── HospitalProfile.cs

Hospital.Infrastructure.InMemory
└── Repositories/
    ├── DoctorInMemoryRepository.cs
    ├── PatientInMemoryRepository.cs
    └── AppointmentInMemoryRepository.cs

Hospital.Api.Host
├── Controllers/
│   ├── DoctorController.cs
│   ├── PatientController.cs
│   ├── AppointmentController.cs
│   └── AnalyticsController.cs
└── Program.cs

Hospital.Tests
├── Queries.cs
├── TestData.cs
└── Tests.cs
```