using MedicalScheduling.Application.Features.Specialities.Commands.UpdateSpeciality;
using MedicalScheduling.UnitTests.Common;

namespace MedicalScheduling.UnitTests.Application.Validators.Specialities;

public sealed class UpdateSpecialityValidatorTests
{
  private readonly UpdateSpecialityValidator _validator = new();

  [Fact]
  public void Validate_WithValidCommand_ShouldNotHaveErrors()
  {
    var result = _validator.Validate(new UpdateSpecialityCommand(
        Guid.NewGuid(),
        TestData.SpecialityName,
        "Descrição"));

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Validate_WithEmptyId_ShouldHaveError()
  {
    var result = _validator.Validate(new UpdateSpecialityCommand(
        Guid.Empty,
        TestData.SpecialityName,
        null));

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateSpecialityCommand.Id));
  }

  [Fact]
  public void Validate_WithEmptyName_ShouldHaveError()
  {
    var result = _validator.Validate(new UpdateSpecialityCommand(Guid.NewGuid(), "", null));

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateSpecialityCommand.Name));
  }
}
