using MediatR;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Features.Doctors.Queries.GetDoctorsBySpeciality;

public sealed record GetDoctorsBySpecialityQuery(Guid SpecialityId) : IRequest<Result<IEnumerable<DoctorDto>>>;
