using MedicalScheduling.Domain;

namespace MedicalScheduling.UnitTests.Domain.Entities;

public sealed class SpecialityTests
{
  [Fact]
  public void Create_WithValidName_ShouldSucceed()
  {
    var result = Speciality.Create("Dermatologia", "Descrição");

    result.IsSuccess.Should().BeTrue();
    result.Value.Name.Should().Be("Dermatologia");
  }

  [Fact]
  public void Create_WithEmptyName_ShouldFail()
  {
    var result = Speciality.Create("  ", null);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Speciality.DuplicateName");
  }

  [Fact]
  public void Update_ShouldChangeProperties()
  {
    var speciality = Speciality.Create("Ortopedia", "Antiga").Value;

    speciality.Update("Ortopedia Pediátrica", "Nova");

    speciality.Name.Should().Be("Ortopedia Pediátrica");
    speciality.Description.Should().Be("Nova");
    speciality.UpdatedAt.Should().NotBeNull();
  }
}
