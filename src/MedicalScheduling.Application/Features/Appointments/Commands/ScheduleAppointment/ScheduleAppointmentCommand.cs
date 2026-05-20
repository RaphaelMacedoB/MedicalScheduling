using MediatR;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Appointments.ScheduleAppointment;

public sealed record ScheduleAppointmentCommand(
    Guid PatientId,
    Guid DoctorId,
    DateTime Start,
    DateTime End) : IRequest<Result<AppointmentDto>>;