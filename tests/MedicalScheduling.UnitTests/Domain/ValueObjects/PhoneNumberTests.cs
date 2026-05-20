using MedicalScheduling.Domain.ValueObjects;
using MedicalScheduling.UnitTests.Common;

namespace MedicalScheduling.UnitTests.Domain.ValueObjects;

public sealed class PhoneNumberTests
{
  [Theory]
  [InlineData(TestData.ValidPhone)]
  [InlineData(TestData.ValidPhoneFormatted)]
  [InlineData("2133334444")]
  public void Create_WithValidPhone_ShouldSucceed(string phone)
  {
    var result = PhoneNumber.Create(phone);

    result.IsSuccess.Should().BeTrue();
    result.Value.Value.Length.Should().BeInRange(10, 11);
  }

  [Theory]
  [InlineData(TestData.InvalidPhone)]
  [InlineData("123456789012")]
  public void Create_WithInvalidPhone_ShouldFail(string phone)
  {
    var result = PhoneNumber.Create(phone);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Patient.InvalidPhone");
  }
}
