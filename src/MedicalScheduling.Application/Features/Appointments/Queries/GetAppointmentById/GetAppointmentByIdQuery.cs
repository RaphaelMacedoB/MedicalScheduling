using MediatR;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Appointments.GetAppointmentById;

public sealed record GetAppointmentByIdQuery(Guid Id) : IRequest<Result<AppointmentDto>>;