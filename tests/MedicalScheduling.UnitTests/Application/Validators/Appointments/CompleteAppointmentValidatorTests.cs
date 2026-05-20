using MedicalScheduling.Application.Features.Appointments.CompleteAppointment;

namespace MedicalScheduling.UnitTests.Application.Validators.Appointments;

public sealed class CompleteAppointmentValidatorTests
{
  private readonly CompleteAppointmentValidator _validator = new();

  [Fact]
  public void Validate_WithValidCommand_ShouldNotHaveErrors()
  {
    var result = _validator.Validate(new CompleteAppointmentCommand(Guid.NewGuid(), "Notas"));

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Validate_WithEmptyId_ShouldHaveError()
  {
    var result = _validator.Validate(new CompleteAppointmentCommand(Guid.Empty, null));

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(CompleteAppointmentCommand.Id));
  }

  [Fact]
  public void Validate_WithNotesTooLong_ShouldHaveError()
  {
    var result = _validator.Validate(new CompleteAppointmentCommand(
        Guid.NewGuid(),
        new string('x', 1001)));

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(CompleteAppointmentCommand.Notes));
  }
}
