using MediatR;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.Infrastructure;

namespace MedicalScheduling.Application.Features.Appointments.CancelAppointment;

public sealed class CancelAppointmentHandler : IRequestHandler<CancelAppointmentCommand, Result>
{
  private readonly IAppointmentRepository _repository;
  private readonly IUnitOfWork _uow;

  public CancelAppointmentHandler(IAppointmentRepository repository, IUnitOfWork uow)
  {
    _repository = repository;
    _uow = uow;
  }

  public async Task<Result> Handle(CancelAppointmentCommand request, CancellationToken ct)
  {
    var appointment = await _repository.GetByIdAsync(request.Id, ct);
    if (appointment is null)
      return Result.Failure(DomainErrors.Appointment.NotFound);

    var result = appointment.Cancel();
    if (result.IsFailure) return result;

    _repository.Update(appointment);
    await _uow.SaveChangesAsync(ct);

    return Result.Success();
  }
}