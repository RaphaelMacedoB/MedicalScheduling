using FluentValidation;

namespace MedicalScheduling.Application.Features.Appointments.ConfirmAppointment;

public sealed class ConfirmAppointmentValidator : AbstractValidator<ConfirmAppointmentCommand>
{
  public ConfirmAppointmentValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty().WithMessage("Id é obrigatório");
  }
}