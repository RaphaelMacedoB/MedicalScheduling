using FluentValidation;

namespace MedicalScheduling.Application.Features.Doctors.Commands.DeactivateDoctor;

public sealed class DeactivateDoctorValidator : AbstractValidator<DeactivateDoctorCommand>
{
  public DeactivateDoctorValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty().WithMessage("Id é obrigatório");
  }
}