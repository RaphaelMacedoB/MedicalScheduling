using MediatR;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Patients.Queries.GetPatientById;

public sealed record GetPatientByIdQuery(Guid Id) : IRequest<Result<PatientDto>>;
