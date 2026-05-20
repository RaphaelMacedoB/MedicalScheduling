namespace MedicalScheduling.Application.Features.Appointments.DTOs;

public sealed record AppointmentDto(
    Guid Id,
    Guid PatientId,
    string PatientName,
    Guid DoctorId,
    string DoctorName,
    string SpecialityName,
    DateTime Start,
    DateTime End,
    string Status,
    string? Notes,
    DateTime CreatedAt);