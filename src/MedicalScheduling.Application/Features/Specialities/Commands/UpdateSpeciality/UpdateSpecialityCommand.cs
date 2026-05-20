namespace MedicalScheduling.Application.Features.Specialities.Commands.UpdateSpeciality;

using MediatR;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain.Primitives;

public sealed record UpdateSpecialityCommand(
    Guid Id,
    string Name,
    string? Description) : IRequest<Result<SpecialityDto>>;
