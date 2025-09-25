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
    
## Структура решения
```
Hospital.Domain
├── Entities
│ ├── Appointment.cs
│ ├── Doctor.cs
│ └── Patient.cs
└── Enums
├── BloodGroup.cs
├── DoctorSpecialization.cs
├── Gender.cs
└── RhesusFactor.cs

Hospital.Tests
├── Queries.cs
├── TestData.cs
└── Tests.cs
<<<<<<< HEAD
```
=======
```
>>>>>>> f3d4cedb69ee60b8aa3c8b4546d3bf20c22511cb
