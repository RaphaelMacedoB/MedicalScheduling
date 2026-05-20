using MedicalScheduling.Application.Features.Appointments.ScheduleAppointment;
using MedicalScheduling.UnitTests.Common;

namespace MedicalScheduling.UnitTests.Application.Validators.Appointments;

public sealed class ScheduleAppointmentValidatorTests
{
  private readonly ScheduleAppointmentValidator _validator = new();

  [Fact]
  public void Validate_WithValidCommand_ShouldNotHaveErrors()
  {
    var command = new ScheduleAppointmentCommand(
        Guid.NewGuid(),
        Guid.NewGuid(),
        TestData.FutureStart,
        TestData.FutureEnd);

    var result = _validator.Validate(command);

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Validate_WithEmptyPatientId_ShouldHaveError()
  {
    var command = new ScheduleAppointmentCommand(
        Guid.Empty,
        Guid.NewGuid(),
        TestData.FutureStart,
        TestData.FutureEnd);

    var result = _validator.Validate(command);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(ScheduleAppointmentCommand.PatientId));
  }

  [Fact]
  public void Validate_WithPastStart_ShouldHaveError()
  {
    var command = new ScheduleAppointmentCommand(
        Guid.NewGuid(),
        Guid.NewGuid(),
        DateTime.UtcNow.AddHours(-1),
        DateTime.UtcNow.AddHours(1));

    var result = _validator.Validate(command);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(ScheduleAppointmentCommand.Start));
  }

  [Fact]
  public void Validate_WhenEndIsBeforeStart_ShouldHaveError()
  {
    var start = TestData.FutureStart;
    var command = new ScheduleAppointmentCommand(
        Guid.NewGuid(),
        Guid.NewGuid(),
        start,
        start.AddHours(-1));

    var result = _validator.Validate(command);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(ScheduleAppointmentCommand.End));
  }
}
