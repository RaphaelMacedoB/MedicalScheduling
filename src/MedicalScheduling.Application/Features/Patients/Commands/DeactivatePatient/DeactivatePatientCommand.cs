namespace MedicalScheduling.Application.Features.Patients.Commands.DeactivatePatient;

using MediatR;
using MedicalScheduling.Domain.Primitives;

public sealed record DeactivatePatientCommand(Guid Id) : IRequest<Result>;