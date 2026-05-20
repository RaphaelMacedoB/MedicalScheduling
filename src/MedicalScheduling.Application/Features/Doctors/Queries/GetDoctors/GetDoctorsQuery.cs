using MediatR;
using MedicalScheduling.Application.Common.Pagination;
using MedicalScheduling.Application.Common.Pagination.Filters;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Doctors.Queries.GetDoctors;

public sealed record GetDoctorsQuery(DoctorFilter Filter)
    : IRequest<Result<PagedResponse<DoctorDto>>>;
