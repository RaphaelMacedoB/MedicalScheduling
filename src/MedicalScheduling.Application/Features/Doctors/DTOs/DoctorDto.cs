namespace MedicalScheduling.Application.Features.Doctors.DTOs;


public sealed record DoctorDto(
    Guid Id,
    string Name,
    string Crm,
    string Email,
    string Phone,
    Guid SpecialityId,
    string SpecialityName,
    bool IsActive,
    DateTime CreatedAt);