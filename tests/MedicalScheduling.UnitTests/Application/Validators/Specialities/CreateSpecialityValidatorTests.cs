using MedicalScheduling.Application.Features.Specialities.Commands.CreateSpeciality;
using MedicalScheduling.UnitTests.Common;

namespace MedicalScheduling.UnitTests.Application.Validators.Specialities;

public sealed class CreateSpecialityValidatorTests
{
  private readonly CreateSpecialityValidator _validator = new();

  [Fact]
  public void Validate_WithValidCommand_ShouldNotHaveErrors()
  {
    var result = _validator.Validate(new CreateSpecialityCommand(TestData.SpecialityName, "Descrição"));

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Validate_WithEmptyName_ShouldHaveError()
  {
    var result = _validator.Validate(new CreateSpecialityCommand("", null));

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateSpecialityCommand.Name));
  }

  [Fact]
  public void Validate_WithDescriptionTooLong_ShouldHaveError()
  {
    var result = _validator.Validate(new CreateSpecialityCommand(
        TestData.SpecialityName,
        new string('x', 501)));

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateSpecialityCommand.Description));
  }
}
