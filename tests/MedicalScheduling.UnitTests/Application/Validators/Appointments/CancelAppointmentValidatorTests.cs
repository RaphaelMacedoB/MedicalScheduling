using MedicalScheduling.Application.Features.Appointments.CancelAppointment;

namespace MedicalScheduling.UnitTests.Application.Validators.Appointments;

public sealed class CancelAppointmentValidatorTests
{
  private readonly CancelAppointmentValidator _validator = new();

  [Fact]
  public void Validate_WithValidId_ShouldNotHaveErrors()
  {
    var result = _validator.Validate(new CancelAppointmentCommand(Guid.NewGuid()));

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Validate_WithEmptyId_ShouldHaveError()
  {
    var result = _validator.Validate(new CancelAppointmentCommand(Guid.Empty));

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(CancelAppointmentCommand.Id));
  }
}
