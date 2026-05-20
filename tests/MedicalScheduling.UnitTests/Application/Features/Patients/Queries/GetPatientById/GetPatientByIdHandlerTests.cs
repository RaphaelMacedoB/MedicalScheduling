using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Patients.Queries.GetPatientById;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Patients.Queries.GetPatientById;

public sealed class GetPatientByIdHandlerTests
{
  private readonly Mock<IPatientRepository> _repository = new();
  private readonly GetPatientByIdHandler _handler;

  public GetPatientByIdHandlerTests()
  {
    _handler = new GetPatientByIdHandler(_repository.Object, new NullCacheService(), MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WhenPatientExists_ShouldReturnDto()
  {
    var patient = EntityTestHelper.CreatePatient();
    var query = new GetPatientByIdQuery(patient.Id);

    _repository.Setup(r => r.GetByIdAsync(patient.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(patient);

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Id.Should().Be(patient.Id);
    result.Value.Name.Should().Be(TestData.PatientName);
  }

  [Fact]
  public async Task Handle_WhenPatientNotFound_ShouldReturnNotFound()
  {
    var query = new GetPatientByIdQuery(Guid.NewGuid());

    _repository.Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Patient?)null);

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Patient.NotFound");
  }
}
