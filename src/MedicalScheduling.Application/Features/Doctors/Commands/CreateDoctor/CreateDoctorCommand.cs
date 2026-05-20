namespace MedicalScheduling.Application.Features.Doctors.Commands.CreateDoctor;

using MediatR;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Domain.Primitives;

public sealed record CreateDoctorCommand(
    string Name,
    string Crm,
    string Email,
    string Phone,
    Guid SpecialityId) : IRequest<Result<DoctorDto>>;