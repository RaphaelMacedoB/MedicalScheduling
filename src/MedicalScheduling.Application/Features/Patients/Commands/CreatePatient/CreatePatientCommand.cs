namespace MedicalScheduling.Application.Features.Patients.Commands.CreatePatient;

using MediatR;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Domain.Primitives;

public sealed record CreatePatientCommand(
    string Name,
    string Email,
    string Cpf,
    string Phone,
    DateOnly BirthDate) : IRequest<Result<PatientDto>>;