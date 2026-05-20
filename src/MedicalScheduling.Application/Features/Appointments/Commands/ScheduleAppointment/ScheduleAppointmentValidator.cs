namespace MedicalScheduling.Application.Features.Appointments.ScheduleAppointment;

using FluentValidation;

public sealed class ScheduleAppointmentValidator : AbstractValidator<ScheduleAppointmentCommand>
{
  public ScheduleAppointmentValidator()
  {
    RuleFor(x => x.PatientId)
        .NotEmpty().WithMessage("Paciente é obrigatório");

    RuleFor(x => x.DoctorId)
        .NotEmpty().WithMessage("Médico é obrigatório");

    RuleFor(x => x.Start)
        .GreaterThan(DateTime.UtcNow).WithMessage("A data de início deve ser futura");

    RuleFor(x => x.End)
        .GreaterThan(x => x.Start).WithMessage("A data de fim deve ser maior que a de início");
  }
}