namespace MedicalScheduling.Application.Features.Specialities.Queries.GetAllSpecialties;

using MediatR;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain.Primitives;

public sealed record GetAllSpecialitiesQuery : IRequest<Result<IEnumerable<SpecialityDto>>>;