using MediatR;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.Infrastructure;

namespace MedicalScheduling.Application.Features.Doctors.Commands.DeactivateDoctor;

public sealed class DeactivateDoctorHandler : IRequestHandler<DeactivateDoctorCommand, Result>
{
  private readonly IDoctorRepository _repository;
  private readonly IUnitOfWork _uow;

  public DeactivateDoctorHandler(IDoctorRepository repository, IUnitOfWork uow)
  {
    _repository = repository;
    _uow = uow;
  }

  public async Task<Result> Handle(DeactivateDoctorCommand request, CancellationToken ct)
  {
    var doctor = await _repository.GetByIdAsync(request.Id, ct);
    if (doctor is null)
      return Result.Failure(DomainErrors.Doctor.NotFound);

    doctor.Deactivate();
    _repository.Update(doctor);
    await _uow.SaveChangesAsync(ct);

    return Result.Success();
  }
}