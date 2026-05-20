namespace MedicalScheduling.Application.Features.Specialities.Queries.GetAllSpecialities;

using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Abstractions.Caching;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

public sealed class GetAllSpecialitiesHandler
    : IRequestHandler<GetAllSpecialitiesQuery, Result<IEnumerable<SpecialityDto>>>
{
  private readonly ISpecialityRepository _repository;
  private readonly ICacheService _cache;
  private readonly IMapper _mapper;

  public GetAllSpecialitiesHandler(
      ISpecialityRepository repository,
      ICacheService cache,
      IMapper mapper)
  {
    _repository = repository;
    _cache = cache;
    _mapper = mapper;
  }

  public async Task<Result<IEnumerable<SpecialityDto>>> Handle(GetAllSpecialitiesQuery request, CancellationToken ct)
  {
    var cached = await _cache.GetAsync<List<SpecialityDto>>(CacheKeys.Specialities.All, ct);
    if (cached is not null)
      return Result.Success<IEnumerable<SpecialityDto>>(cached);

    var specialities = await _repository.GetAllAsync(ct);
    var dtos = _mapper.Map<List<SpecialityDto>>(specialities);
    await _cache.SetAsync(CacheKeys.Specialities.All, dtos, cancellationToken: ct);

    return Result.Success<IEnumerable<SpecialityDto>>(dtos);
  }
}
