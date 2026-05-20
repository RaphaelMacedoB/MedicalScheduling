using MedicalScheduling.Domain.ValueObjects;
using MedicalScheduling.UnitTests.Common;

namespace MedicalScheduling.UnitTests.Domain.ValueObjects;

public sealed class CpfTests
{
  [Theory]
  [InlineData(TestData.ValidCpf)]
  [InlineData(TestData.ValidCpfFormatted)]
  [InlineData("11144477735")]
  public void Create_WithValidCpf_ShouldSucceed(string cpf)
  {
    var result = Cpf.Create(cpf);

    result.IsSuccess.Should().BeTrue();
    result.Value.Value.Should().HaveLength(11);
  }

  [Theory]
  [InlineData("")]
  [InlineData("   ")]
  [InlineData(TestData.InvalidCpf)]
  [InlineData("123")]
  public void Create_WithInvalidCpf_ShouldFail(string cpf)
  {
    var result = Cpf.Create(cpf);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Patient.InvalidCpf");
  }

  [Fact]
  public void Equals_WithSameValue_ShouldBeEqual()
  {
    var cpf1 = Cpf.Create(TestData.ValidCpf).Value;
    var cpf2 = Cpf.Create(TestData.ValidCpfFormatted).Value;

    cpf1.Should().Be(cpf2);
  }
}
