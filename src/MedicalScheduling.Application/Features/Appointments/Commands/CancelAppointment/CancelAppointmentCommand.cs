using MediatR;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Appointments.CancelAppointment;

public sealed record CancelAppointmentCommand(Guid Id) : IRequest<Result>;