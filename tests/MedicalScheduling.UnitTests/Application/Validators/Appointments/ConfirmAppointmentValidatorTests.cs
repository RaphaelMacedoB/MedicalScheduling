using MedicalScheduling.Application.Features.Appointments.ConfirmAppointment;

namespace MedicalScheduling.UnitTests.Application.Validators.Appointments;

public sealed class ConfirmAppointmentValidatorTests
{
  private readonly ConfirmAppointmentValidator _validator = new();

  [Fact]
  public void Validate_WithValidId_ShouldNotHaveErrors()
  {
    var result = _validator.Validate(new ConfirmAppointmentCommand(Guid.NewGuid()));

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Validate_WithEmptyId_ShouldHaveError()
  {
    var result = _validator.Validate(new ConfirmAppointmentCommand(Guid.Empty));

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(ConfirmAppointmentCommand.Id));
  }
}
