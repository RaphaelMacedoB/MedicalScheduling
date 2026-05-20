using MedicalScheduling.Application.Features.Patients.Commands.CreatePatient;
using MedicalScheduling.UnitTests.Common;

namespace MedicalScheduling.UnitTests.Application.Validators.Patients;

public sealed class CreatePatientValidatorTests
{
  private readonly CreatePatientValidator _validator = new();

  [Fact]
  public void Validate_WithValidCommand_ShouldNotHaveErrors()
  {
    var command = new CreatePatientCommand(
        TestData.PatientName,
        TestData.ValidEmail,
        TestData.ValidCpf,
        TestData.ValidPhone,
        TestData.ValidBirthDate);

    var result = _validator.Validate(command);

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Validate_WithEmptyName_ShouldHaveError()
  {
    var command = new CreatePatientCommand(
        "",
        TestData.ValidEmail,
        TestData.ValidCpf,
        TestData.ValidPhone,
        TestData.ValidBirthDate);

    var result = _validator.Validate(command);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(CreatePatientCommand.Name));
  }

  [Fact]
  public void Validate_WithInvalidEmail_ShouldHaveError()
  {
    var command = new CreatePatientCommand(
        TestData.PatientName,
        TestData.InvalidEmail,
        TestData.ValidCpf,
        TestData.ValidPhone,
        TestData.ValidBirthDate);

    var result = _validator.Validate(command);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(CreatePatientCommand.Email));
  }

  [Fact]
  public void Validate_WithFutureBirthDate_ShouldHaveError()
  {
    var command = new CreatePatientCommand(
        TestData.PatientName,
        TestData.ValidEmail,
        TestData.ValidCpf,
        TestData.ValidPhone,
        DateOnly.FromDateTime(DateTime.Today));

    var result = _validator.Validate(command);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(CreatePatientCommand.BirthDate));
  }
}
