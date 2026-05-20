using MediatR;
using MedicalScheduling.Application.Abstractions.Caching;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;
namespace MedicalScheduling.Application.Features.Patients.Commands.DeactivatePatient;

public sealed class DeactivatePatientHandler : IRequestHandler<DeactivatePatientCommand, Result>
{
  private readonly IPatientRepository _repository;
  private readonly IUnitOfWork _uow;
  private readonly ICacheService _cache;

  public DeactivatePatientHandler(IPatientRepository repository, IUnitOfWork uow, ICacheService cache)
  {
    _repository = repository;
    _uow = uow;
    _cache = cache;
  }

  public async Task<Result> Handle(DeactivatePatientCommand request, CancellationToken ct)
  {
    var patient = await _repository.GetByIdAsync(request.Id, ct);
    if (patient is null)
      return Result.Failure(DomainErrors.Patient.NotFound);

    patient.Deactivate();
    _repository.Update(patient);
    await _uow.SaveChangesAsync(ct);
    await _cache.RemoveAsync(CacheKeys.Patients.ById(request.Id), ct);

    return Result.Success();
  }
}