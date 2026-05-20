using MediatR;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;
namespace MedicalScheduling.Application.Features.Patients.Commands.DeactivatePatient;

public sealed class DeactivatePatientHandler : IRequestHandler<DeactivatePatientCommand, Result>
{
  private readonly IPatientRepository _repository;
  private readonly IUnitOfWork _uow;

  public DeactivatePatientHandler(IPatientRepository repository, IUnitOfWork uow)
  {
    _repository = repository;
    _uow = uow;
  }

  public async Task<Result> Handle(DeactivatePatientCommand request, CancellationToken ct)
  {
    var patient = await _repository.GetByIdAsync(request.Id, ct);
    if (patient is null)
      return Result.Failure(DomainErrors.Patient.NotFound);

    patient.Deactivate();
    _repository.Update(patient);
    await _uow.SaveChangesAsync(ct);

    return Result.Success();
  }
}