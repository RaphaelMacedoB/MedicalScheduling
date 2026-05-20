using FluentValidation;

namespace MedicalScheduling.Application.Features.Appointments.CompleteAppointment;

public sealed class CompleteAppointmentValidator : AbstractValidator<CompleteAppointmentCommand>
{
  public CompleteAppointmentValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty().WithMessage("Id é obrigatório");

    RuleFor(x => x.Notes)
        .MaximumLength(1000).WithMessage("Observações devem ter no máximo 1000 caracteres")
        .When(x => x.Notes is not null);
  }
}