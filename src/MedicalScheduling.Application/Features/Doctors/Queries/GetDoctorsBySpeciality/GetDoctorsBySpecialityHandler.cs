using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Abstractions.Caching;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Doctors.Queries.GetDoctorsBySpeciality;

public sealed class GetDoctorsBySpecialityHandler
    : IRequestHandler<GetDoctorsBySpecialityQuery, Result<IEnumerable<DoctorDto>>>
{
  private readonly IDoctorRepository _doctorRepository;
  private readonly ISpecialityRepository _specialityRepository;
  private readonly ICacheService _cache;
  private readonly IMapper _mapper;

  public GetDoctorsBySpecialityHandler(
      IDoctorRepository doctorRepository,
      ISpecialityRepository specialityRepository,
      ICacheService cache,
      IMapper mapper)
  {
    _doctorRepository = doctorRepository;
    _specialityRepository = specialityRepository;
    _cache = cache;
    _mapper = mapper;
  }

  public async Task<Result<IEnumerable<DoctorDto>>> Handle(GetDoctorsBySpecialityQuery request, CancellationToken ct)
  {
    var cacheKey = CacheKeys.Doctors.BySpeciality(request.SpecialityId);
    var cached = await _cache.GetAsync<List<DoctorDto>>(cacheKey, ct);
    if (cached is not null)
      return Result.Success<IEnumerable<DoctorDto>>(cached);

    var speciality = await _specialityRepository.GetByIdAsync(request.SpecialityId, ct);
    if (speciality is null)
      return Result.Failure<IEnumerable<DoctorDto>>(DomainErrors.Speciality.NotFound);

    var doctors = await _doctorRepository.GetBySpecialityAsync(request.SpecialityId, ct);
    var dtos = _mapper.Map<List<DoctorDto>>(doctors);
    await _cache.SetAsync(cacheKey, dtos, cancellationToken: ct);

    return Result.Success<IEnumerable<DoctorDto>>(dtos);
  }
}
