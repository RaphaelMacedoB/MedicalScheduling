using MediatR;
using MedicalScheduling.Application.Common.Pagination;
using MedicalScheduling.Application.Common.Pagination.Filters;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Patients.Queries.GetPatients;

public sealed record GetPatientsQuery(PatientFilter Filter)
    : IRequest<Result<PagedResponse<PatientDto>>>;
