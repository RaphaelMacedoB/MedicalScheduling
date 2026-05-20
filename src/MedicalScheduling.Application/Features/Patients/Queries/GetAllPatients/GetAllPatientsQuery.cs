namespace MedicalScheduling.Application.Features.Patients.Queries.GetAllPatients;

using MediatR;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Domain.Primitives;

public sealed record GetAllPatientsQuery : IRequest<Result<IEnumerable<PatientDto>>>;
