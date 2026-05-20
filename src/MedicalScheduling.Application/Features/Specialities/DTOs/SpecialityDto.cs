namespace MedicalScheduling.Application.Features.Specialities.DTOs;

public sealed record SpecialityDto(
  Guid Id,
  string Name,
  string? Description,
  DateTime CreatedAt);