using MedicalScheduling.Application.Common.Pagination;
using MedicalScheduling.Application.Common.Pagination.Filters;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Application.Features.Patients.Queries.GetPatients;
using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Common;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Patients.Queries.GetPatients;

public sealed class GetPatientsHandlerTests
{
  private readonly Mock<IPatientRepository> _repository = new();
  private readonly GetPatientsHandler _handler;

  public GetPatientsHandlerTests()
  {
    _handler = new GetPatientsHandler(_repository.Object, MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WhenPatientsExist_ShouldReturnPagedDtos()
  {
    var patients = new[] { EntityTestHelper.CreatePatient(), EntityTestHelper.CreatePatient() };
    _repository.Setup(r => r.GetPagedAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<bool?>(),
            It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>(), It.IsAny<string?>(), It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(new PagedResult<Patient> { Items = patients, TotalCount = 2 });

    var result = await _handler.Handle(new GetPatientsQuery(new PatientFilter()), CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Items.Should().HaveCount(2);
    result.Value.TotalItems.Should().Be(2);
  }
}
