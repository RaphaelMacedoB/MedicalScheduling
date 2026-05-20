using FluentValidation;

namespace MedicalScheduling.Application.Features.Appointments.CancelAppointment;

public sealed class CancelAppointmentValidator : AbstractValidator<CancelAppointmentCommand>
{
  public CancelAppointmentValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty().WithMessage("Id é obrigatório");
  }
}