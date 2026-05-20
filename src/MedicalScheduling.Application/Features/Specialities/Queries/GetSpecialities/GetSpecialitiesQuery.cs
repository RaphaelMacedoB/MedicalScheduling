using MediatR;
using MedicalScheduling.Application.Common.Pagination;
using MedicalScheduling.Application.Common.Pagination.Filters;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Specialities.Queries.GetSpecialities;

public sealed record GetSpecialitiesQuery(SpecialityFilter Filter)
    : IRequest<Result<PagedResponse<SpecialityDto>>>;
