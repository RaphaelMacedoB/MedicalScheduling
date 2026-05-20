namespace MedicalScheduling.Application.Features.Specialities.Commands.UpdateSpeciality;

using FluentValidation;

public sealed class UpdateSpecialtyValidator : AbstractValidator<UpdateSpecialtyCommand>
{
  public UpdateSpecialtyValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty().WithMessage("Id é obrigatório");

    RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Nome é obrigatório")
        .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

    RuleFor(x => x.Description)
        .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres")
        .When(x => x.Description is not null);
  }
}