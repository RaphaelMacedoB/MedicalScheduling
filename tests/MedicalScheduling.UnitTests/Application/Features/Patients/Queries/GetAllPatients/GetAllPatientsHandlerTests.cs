using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Patients.Queries.GetAllPatients;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Patients.Queries.GetAllPatients;

public sealed class GetAllPatientsHandlerTests
{
  private readonly Mock<IPatientRepository> _repository = new();
  private readonly GetAllPatientsHandler _handler;

  public GetAllPatientsHandlerTests()
  {
    _handler = new GetAllPatientsHandler(_repository.Object, new NullCacheService(), MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WhenPatientsExist_ShouldReturnMappedDtos()
  {
    var patients = new[]
    {
      EntityTestHelper.CreatePatient(),
      EntityTestHelper.CreatePatient(cpf: "39053344705", email: TestData.UniqueEmail())
    };

    _repository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(patients);

    var result = await _handler.Handle(new GetAllPatientsQuery(), CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().HaveCount(2);
    result.Value.First().Name.Should().Be(TestData.PatientName);
  }

  [Fact]
  public async Task Handle_WhenNoPatients_ShouldReturnEmptyList()
  {
    _repository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(Array.Empty<Patient>());

    var result = await _handler.Handle(new GetAllPatientsQuery(), CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().BeEmpty();
  }
}
