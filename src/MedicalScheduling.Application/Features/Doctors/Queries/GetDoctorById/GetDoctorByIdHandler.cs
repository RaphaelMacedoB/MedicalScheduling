namespace MedicalScheduling.Application.Features.Doctors.Queries.GetDoctorById;

using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Abstractions.Caching;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;


public sealed class GetDoctorByIdHandler : IRequestHandler<GetDoctorByIdQuery, Result<DoctorDto>>
{
  private readonly IDoctorRepository _repository;
  private readonly ICacheService _cache;
  private readonly IMapper _mapper;

  public GetDoctorByIdHandler(IDoctorRepository repository, ICacheService cache, IMapper mapper)
  {
    _repository = repository;
    _cache = cache;
    _mapper = mapper;
  }

  public async Task<Result<DoctorDto>> Handle(GetDoctorByIdQuery request, CancellationToken ct)
  {
    var cacheKey = CacheKeys.Doctors.ById(request.Id);
    var cached = await _cache.GetAsync<DoctorDto>(cacheKey, ct);
    if (cached is not null)
      return Result.Success(cached);

    var doctor = await _repository.GetByIdAsync(request.Id, ct);
    if (doctor is null)
      return Result.Failure<DoctorDto>(DomainErrors.Doctor.NotFound);

    var dto = _mapper.Map<DoctorDto>(doctor);
    await _cache.SetAsync(cacheKey, dto, cancellationToken: ct);

    return Result.Success(dto);
  }
}
