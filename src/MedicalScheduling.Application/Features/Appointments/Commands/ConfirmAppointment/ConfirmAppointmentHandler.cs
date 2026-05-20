using MediatR;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.Infrastructure;

namespace MedicalScheduling.Application.Features.Appointments.ConfirmAppointment;

public sealed class ConfirmAppointmentHandler : IRequestHandler<ConfirmAppointmentCommand, Result>
{
  private readonly IAppointmentRepository _repository;
  private readonly IUnitOfWork _uow;

  public ConfirmAppointmentHandler(IAppointmentRepository repository, IUnitOfWork uow)
  {
    _repository = repository;
    _uow = uow;
  }

  public async Task<Result> Handle(ConfirmAppointmentCommand request, CancellationToken ct)
  {
    var appointment = await _repository.GetByIdAsync(request.Id, ct);
    if (appointment is null)
      return Result.Failure(DomainErrors.Appointment.NotFound);

    var result = appointment.Confirm();
    if (result.IsFailure) return result;

    _repository.Update(appointment);
    await _uow.SaveChangesAsync(ct);

    return Result.Success();
  }
}