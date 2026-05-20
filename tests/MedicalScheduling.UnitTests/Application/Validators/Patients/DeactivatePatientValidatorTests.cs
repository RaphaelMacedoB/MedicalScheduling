using MedicalScheduling.Application.Features.Patients.Commands.DeactivatePatient;

namespace MedicalScheduling.UnitTests.Application.Validators.Patients;

public sealed class DeactivatePatientValidatorTests
{
  private readonly DeactivatePatientValidator _validator = new();

  [Fact]
  public void Validate_WithValidId_ShouldNotHaveErrors()
  {
    var result = _validator.Validate(new DeactivatePatientCommand(Guid.NewGuid()));

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Validate_WithEmptyId_ShouldHaveError()
  {
    var result = _validator.Validate(new DeactivatePatientCommand(Guid.Empty));

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(DeactivatePatientCommand.Id));
  }
}
