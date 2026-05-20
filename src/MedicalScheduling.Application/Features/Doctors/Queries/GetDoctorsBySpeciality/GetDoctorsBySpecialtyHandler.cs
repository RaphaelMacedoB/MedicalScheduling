using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Doctors.Queries.GetDoctorsBySpeciality;

public sealed class GetDoctorsBySpecialityHandler
    : IRequestHandler<GetDoctorsBySpecialityQuery, Result<IEnumerable<DoctorDto>>>
{
  private readonly IDoctorRepository _doctorRepository;
  private readonly ISpecialityRepository _specialityRepository;
  private readonly IMapper _mapper;

  public GetDoctorsBySpecialityHandler(
      IDoctorRepository doctorRepository,
      ISpecialityRepository specialityRepository,
      IMapper mapper)
  {
    _doctorRepository = doctorRepository;
    _specialityRepository = specialityRepository;
    _mapper = mapper;
  }

  public async Task<Result<IEnumerable<DoctorDto>>> Handle(GetDoctorsBySpecialityQuery request, CancellationToken ct)
  {
    var speciality = await _specialityRepository.GetByIdAsync(request.SpecialityId, ct);
    if (speciality is null)
      return Result.Failure<IEnumerable<DoctorDto>>(DomainErrors.Speciality.NotFound);

    var doctors = await _doctorRepository.GetBySpecialityAsync(request.SpecialityId, ct);
    return Result.Success(_mapper.Map<IEnumerable<DoctorDto>>(doctors));
  }
}