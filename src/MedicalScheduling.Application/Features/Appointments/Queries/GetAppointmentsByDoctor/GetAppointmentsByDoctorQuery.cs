using MediatR;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Appointments.GetAppointmentsByDoctor;

public sealed record GetAppointmentsByDoctorQuery(Guid DoctorId) : IRequest<Result<IEnumerable<AppointmentDto>>>;