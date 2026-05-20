using MediatR;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Appointments.CompleteAppointment;

public sealed record CompleteAppointmentCommand(Guid Id, string? Notes) : IRequest<Result>;