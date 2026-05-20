namespace MedicalScheduling.Application.Features.Doctors.Commands.DeactivateDoctor;

using MediatR;
using MedicalScheduling.Domain.Primitives;

public sealed record DeactivateDoctorCommand(Guid Id) : IRequest<Result>;