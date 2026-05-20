using FluentValidation;

namespace MedicalScheduling.Application.Features.Patients.Commands.CreatePatient;

public sealed class CreatePatientValidator : AbstractValidator<CreatePatientCommand>
{
  public CreatePatientValidator()
  {
    RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Nome é obrigatório")
        .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("E-mail é obrigatório")
        .EmailAddress().WithMessage("E-mail inválido");

    RuleFor(x => x.Cpf)
        .NotEmpty().WithMessage("CPF é obrigatório")
        .Length(11, 14).WithMessage("CPF inválido");

    RuleFor(x => x.Phone)
        .NotEmpty().WithMessage("Telefone é obrigatório")
        .MinimumLength(10).WithMessage("Telefone inválido");

    RuleFor(x => x.BirthDate)
        .LessThan(DateOnly.FromDateTime(DateTime.Today))
        .WithMessage("Data de nascimento inválida");
  }
}