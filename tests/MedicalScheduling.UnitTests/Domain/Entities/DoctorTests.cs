using MedicalScheduling.Domain;
using MedicalScheduling.UnitTests.Common;

namespace MedicalScheduling.UnitTests.Domain.Entities;

public sealed class DoctorTests
{
  [Fact]
  public void Create_WithValidData_ShouldSucceed()
  {
    var result = Doctor.Create(
        TestData.DoctorName,
        TestData.Crm,
        TestData.ValidEmail,
        TestData.ValidPhone,
        Guid.NewGuid());

    result.IsSuccess.Should().BeTrue();
    result.Value.IsActive.Should().BeTrue();
  }

  [Fact]
  public void Create_WithEmptyCrm_ShouldFail()
  {
    var result = Doctor.Create(TestData.DoctorName, "", TestData.ValidEmail, TestData.ValidPhone, Guid.NewGuid());

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Doctor.InvalidCrm");
  }

  [Fact]
  public void Deactivate_ShouldSetInactive()
  {
    var doctor = Doctor.Create(
        TestData.DoctorName,
        TestData.Crm,
        TestData.ValidEmail,
        TestData.ValidPhone,
        Guid.NewGuid()).Value;

    doctor.Deactivate();

    doctor.IsActive.Should().BeFalse();
  }
}
