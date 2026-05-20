using MediatR;
using MedicalScheduling.Application.Abstractions.Caching;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;
namespace MedicalScheduling.Application.Features.Doctors.Commands.DeactivateDoctor;

public sealed class DeactivateDoctorHandler : IRequestHandler<DeactivateDoctorCommand, Result>
{
  private readonly IDoctorRepository _repository;
  private readonly IUnitOfWork _uow;
  private readonly ICacheService _cache;

  public DeactivateDoctorHandler(IDoctorRepository repository, IUnitOfWork uow, ICacheService cache)
  {
    _repository = repository;
    _uow = uow;
    _cache = cache;
  }

  public async Task<Result> Handle(DeactivateDoctorCommand request, CancellationToken ct)
  {
    var doctor = await _repository.GetByIdAsync(request.Id, ct);
    if (doctor is null)
      return Result.Failure(DomainErrors.Doctor.NotFound);

    doctor.Deactivate();
    _repository.Update(doctor);
    await _uow.SaveChangesAsync(ct);
    await _cache.RemoveAsync(CacheKeys.Doctors.ById(request.Id), ct);
    await _cache.RemoveAsync(CacheKeys.Doctors.BySpeciality(doctor.SpecialityId), ct);

    return Result.Success();
  }
}