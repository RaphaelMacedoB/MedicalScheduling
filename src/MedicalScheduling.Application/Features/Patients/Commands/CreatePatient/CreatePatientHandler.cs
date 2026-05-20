using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Abstractions.Caching;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Patients.Commands.CreatePatient;

public sealed class CreatePatientHandler : IRequestHandler<CreatePatientCommand, Result<PatientDto>>
{
  private readonly IPatientRepository _repository;
  private readonly IUnitOfWork _uow;
  private readonly ICacheService _cache;
  private readonly IMapper _mapper;

  public CreatePatientHandler(
      IPatientRepository repository,
      IUnitOfWork uow,
      ICacheService cache,
      IMapper mapper)
  {
    _repository = repository;
    _uow = uow;
    _cache = cache;
    _mapper = mapper;
  }

  public async Task<Result<PatientDto>> Handle(CreatePatientCommand request, CancellationToken ct)
  {
    var exists = await _repository.ExistsByCpfAsync(request.Cpf, ct);
    if (exists)
      return Result.Failure<PatientDto>(DomainErrors.Patient.InvalidCpf);

    var result = Patient.Create(
        request.Name,
        request.Email,
        request.Cpf,
        request.Phone,
        request.BirthDate);

    if (result.IsFailure)
      return Result.Failure<PatientDto>(result.Error);

    await _repository.AddAsync(result.Value, ct);
    await _uow.SaveChangesAsync(ct);
    await _cache.RemoveAsync(CacheKeys.Patients.All, ct);

    return Result.Success(_mapper.Map<PatientDto>(result.Value));
  }
}