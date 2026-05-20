using MedicalScheduling.Application.Features.Doctors.Commands.CreateDoctor;
using MedicalScheduling.UnitTests.Common;

namespace MedicalScheduling.UnitTests.Application.Validators.Doctors;

public sealed class CreateDoctorValidatorTests
{
  private readonly CreateDoctorValidator _validator = new();

  [Fact]
  public void Validate_WithValidCommand_ShouldNotHaveErrors()
  {
    var command = new CreateDoctorCommand(
        TestData.DoctorName,
        TestData.Crm,
        TestData.ValidEmail,
        TestData.ValidPhone,
        Guid.NewGuid());

    var result = _validator.Validate(command);

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Validate_WithEmptyName_ShouldHaveError()
  {
    var command = new CreateDoctorCommand(
        "",
        TestData.Crm,
        TestData.ValidEmail,
        TestData.ValidPhone,
        Guid.NewGuid());

    var result = _validator.Validate(command);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDoctorCommand.Name));
  }

  [Fact]
  public void Validate_WithEmptySpecialityId_ShouldHaveError()
  {
    var command = new CreateDoctorCommand(
        TestData.DoctorName,
        TestData.Crm,
        TestData.ValidEmail,
        TestData.ValidPhone,
        Guid.Empty);

    var result = _validator.Validate(command);

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDoctorCommand.SpecialityId));
  }
}
