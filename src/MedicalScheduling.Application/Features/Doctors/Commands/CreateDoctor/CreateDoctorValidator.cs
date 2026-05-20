namespace MedicalScheduling.Application.Features.Doctors.Commands.CreateDoctor;

using FluentValidation;

public sealed class CreateDoctorValidator : AbstractValidator<CreateDoctorCommand>
{
  public CreateDoctorValidator()
  {
    RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Nome é obrigatório")
        .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

    RuleFor(x => x.Crm)
        .NotEmpty().WithMessage("CRM é obrigatório")
        .MaximumLength(20).WithMessage("CRM inválido");

    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("E-mail é obrigatório")
        .EmailAddress().WithMessage("E-mail inválido");

    RuleFor(x => x.Phone)
        .NotEmpty().WithMessage("Telefone é obrigatório")
        .MinimumLength(10).WithMessage("Telefone inválido");

    RuleFor(x => x.SpecialityId)
        .NotEmpty().WithMessage("Especialidade é obrigatória");
  }
}