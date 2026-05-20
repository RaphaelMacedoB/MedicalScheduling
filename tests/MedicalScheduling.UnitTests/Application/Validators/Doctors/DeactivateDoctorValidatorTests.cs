using MedicalScheduling.Application.Features.Doctors.Commands.DeactivateDoctor;

namespace MedicalScheduling.UnitTests.Application.Validators.Doctors;

public sealed class DeactivateDoctorValidatorTests
{
  private readonly DeactivateDoctorValidator _validator = new();

  [Fact]
  public void Validate_WithValidId_ShouldNotHaveErrors()
  {
    var result = _validator.Validate(new DeactivateDoctorCommand(Guid.NewGuid()));

    result.IsValid.Should().BeTrue();
  }

  [Fact]
  public void Validate_WithEmptyId_ShouldHaveError()
  {
    var result = _validator.Validate(new DeactivateDoctorCommand(Guid.Empty));

    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == nameof(DeactivateDoctorCommand.Id));
  }
}
