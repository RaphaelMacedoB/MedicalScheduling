using MedicalScheduling.Application.Common.Pagination.Filters;
using MedicalScheduling.Application.Features.Specialities.Queries.GetSpecialities;
using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Common;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Specialities.Queries.GetSpecialities;

public sealed class GetSpecialitiesHandlerTests
{
  private readonly Mock<ISpecialityRepository> _repository = new();
  private readonly GetSpecialitiesHandler _handler;

  public GetSpecialitiesHandlerTests()
  {
    _handler = new GetSpecialitiesHandler(_repository.Object, MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WhenSpecialitiesExist_ShouldReturnPagedDtos()
  {
    var specialities = new[] { EntityTestHelper.CreateSpeciality(), EntityTestHelper.CreateSpeciality(name: "Ortopedia") };
    _repository.Setup(r => r.GetPagedAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(new PagedResult<Speciality> { Items = specialities, TotalCount = 2 });

    var result = await _handler.Handle(new GetSpecialitiesQuery(new SpecialityFilter()), CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Items.Should().HaveCount(2);
    result.Value.TotalItems.Should().Be(2);
  }
}
