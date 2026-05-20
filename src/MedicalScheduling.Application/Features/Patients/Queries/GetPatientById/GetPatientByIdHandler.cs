using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Abstractions.Caching;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Patients.Queries.GetPatientById;

public sealed class GetPatientByIdHandler : IRequestHandler<GetPatientByIdQuery, Result<PatientDto>>
{
  private readonly IPatientRepository _repository;
  private readonly ICacheService _cache;
  private readonly IMapper _mapper;

  public GetPatientByIdHandler(IPatientRepository repository, ICacheService cache, IMapper mapper)
  {
    _repository = repository;
    _cache = cache;
    _mapper = mapper;
  }

  public async Task<Result<PatientDto>> Handle(GetPatientByIdQuery request, CancellationToken ct)
  {
    var cacheKey = CacheKeys.Patients.ById(request.Id);
    var cached = await _cache.GetAsync<PatientDto>(cacheKey, ct);
    if (cached is not null)
      return Result.Success(cached);

    var patient = await _repository.GetByIdAsync(request.Id, ct);
    if (patient is null)
      return Result.Failure<PatientDto>(DomainErrors.Patient.NotFound);

    var dto = _mapper.Map<PatientDto>(patient);
    await _cache.SetAsync(cacheKey, dto, cancellationToken: ct);

    return Result.Success(dto);
  }
}