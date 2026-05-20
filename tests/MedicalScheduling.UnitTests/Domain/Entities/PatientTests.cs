using MedicalScheduling.Domain;
using MedicalScheduling.UnitTests.Common;

namespace MedicalScheduling.UnitTests.Domain.Entities;

public sealed class PatientTests
{
  [Fact]
  public void Create_WithValidData_ShouldSucceed()
  {
    var result = Patient.Create(
        TestData.PatientName,
        TestData.ValidEmail,
        TestData.ValidCpf,
        TestData.ValidPhone,
        TestData.ValidBirthDate);

    result.IsSuccess.Should().BeTrue();
    result.Value.Name.Should().Be(TestData.PatientName);
    result.Value.IsActive.Should().BeTrue();
  }

  [Fact]
  public void Create_WithEmptyName_ShouldFail()
  {
    var result = Patient.Create("", TestData.ValidEmail, TestData.ValidCpf, TestData.ValidPhone, TestData.ValidBirthDate);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Patient.InvalidName");
  }

  [Fact]
  public void Deactivate_ShouldSetInactive()
  {
    var patient = Patient.Create(
        TestData.PatientName,
        TestData.ValidEmail,
        TestData.ValidCpf,
        TestData.ValidPhone,
        TestData.ValidBirthDate).Value;

    patient.Deactivate();

    patient.IsActive.Should().BeFalse();
    patient.UpdatedAt.Should().NotBeNull();
  }
}
