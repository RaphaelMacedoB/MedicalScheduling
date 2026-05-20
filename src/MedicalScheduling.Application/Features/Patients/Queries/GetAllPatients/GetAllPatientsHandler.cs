using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Abstractions.Caching;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Patients.Queries.GetAllPatients;

public sealed class GetAllPatientsHandler : IRequestHandler<GetAllPatientsQuery, Result<IEnumerable<PatientDto>>>
{
  private readonly IPatientRepository _repository;
  private readonly ICacheService _cache;
  private readonly IMapper _mapper;

  public GetAllPatientsHandler(IPatientRepository repository, ICacheService cache, IMapper mapper)
  {
    _repository = repository;
    _cache = cache;
    _mapper = mapper;
  }

  public async Task<Result<IEnumerable<PatientDto>>> Handle(GetAllPatientsQuery request, CancellationToken ct)
  {
    var cached = await _cache.GetAsync<List<PatientDto>>(CacheKeys.Patients.All, ct);
    if (cached is not null)
      return Result.Success<IEnumerable<PatientDto>>(cached);

    var patients = await _repository.GetAllAsync(ct);
    var dtos = _mapper.Map<List<PatientDto>>(patients);
    await _cache.SetAsync(CacheKeys.Patients.All, dtos, cancellationToken: ct);

    return Result.Success<IEnumerable<PatientDto>>(dtos);
  }
}
