using MediatR;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Appointments.ConfirmAppointment;

public sealed record ConfirmAppointmentCommand(Guid Id) : IRequest<Result>;