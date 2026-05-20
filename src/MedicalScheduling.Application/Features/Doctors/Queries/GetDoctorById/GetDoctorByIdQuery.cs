using MediatR;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Doctors.Queries.GetDoctorById;

public sealed record GetDoctorByIdQuery(Guid Id) : IRequest<Result<DoctorDto>>;