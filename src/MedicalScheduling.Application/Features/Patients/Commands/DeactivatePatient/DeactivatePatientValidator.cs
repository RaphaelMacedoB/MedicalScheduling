using FluentValidation;

namespace MedicalScheduling.Application.Features.Patients.Commands.DeactivatePatient;

public sealed class DeactivatePatientValidator : AbstractValidator<DeactivatePatientCommand>
{
  public DeactivatePatientValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty().WithMessage("Id é obrigatório");
  }
}