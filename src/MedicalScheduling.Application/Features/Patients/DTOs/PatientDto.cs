namespace MedicalScheduling.Application.Features.Patients.DTOs;

public sealed record PatientDto(
    Guid Id,
    string Name,
    string Email,
    string Cpf,
    string Phone,
    DateOnly BirthDate,
    bool IsActive,
    DateTime CreatedAt);