namespace MedicalScheduling.Application.Features.Specialities.Commands.CreateSpeciality;

using MediatR;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain.Primitives;

public sealed record CreateSpecialtyCommand(
    string Name,
    string? Description) : IRequest<Result<SpecialityDto>>;