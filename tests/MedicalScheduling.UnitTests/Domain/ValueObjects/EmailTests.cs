using MedicalScheduling.Domain.ValueObjects;
using MedicalScheduling.UnitTests.Common;

namespace MedicalScheduling.UnitTests.Domain.ValueObjects;

public sealed class EmailTests
{
  [Fact]
  public void Create_WithValidEmail_ShouldSucceedAndLowercase()
  {
    var result = Email.Create("User@Example.COM");

    result.IsSuccess.Should().BeTrue();
    result.Value.Value.Should().Be("user@example.com");
  }

  [Theory]
  [InlineData("")]
  [InlineData(TestData.InvalidEmail)]
  [InlineData("missing@dot")]
  public void Create_WithInvalidEmail_ShouldFail(string email)
  {
    var result = Email.Create(email);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Patient.InvalidEmail");
  }
}
