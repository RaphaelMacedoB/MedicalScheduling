using MediatR;
using MedicalScheduling.Application.Common.Pagination;
using MedicalScheduling.Application.Common.Pagination.Filters;
using MedicalScheduling.Application.Features.Appointments.DTOs;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Appointments.Queries.GetAppointments;

public sealed record GetAppointmentsQuery(AppointmentFilter Filter)
    : IRequest<Result<PagedResponse<AppointmentDto>>>;
