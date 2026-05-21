using MediatR;
using MedicalScheduling.Application.Features.Appointments.DTOs;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Appointments.GetAppointmentsByPatient;

public sealed record GetAppointmentsByPatientQuery(Guid PatientId) : IRequest<Result<IEnumerable<AppointmentDto>>>;