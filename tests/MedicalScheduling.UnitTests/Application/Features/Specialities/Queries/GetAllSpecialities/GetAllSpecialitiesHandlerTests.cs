using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Specialities.Queries.GetAllSpecialities;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Specialities.Queries.GetAllSpecialities;

public sealed class GetAllSpecialitiesHandlerTests
{
  private readonly Mock<ISpecialityRepository> _repository = new();
  private readonly GetAllSpecialitiesHandler _handler;

  public GetAllSpecialitiesHandlerTests()
  {
    _handler = new GetAllSpecialitiesHandler(_repository.Object, new NullCacheService(), MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WhenSpecialitiesExist_ShouldReturnMappedDtos()
  {
    var specialities = new[]
    {
      EntityTestHelper.CreateSpeciality(),
      EntityTestHelper.CreateSpeciality(name: "Ortopedia")
    };

    _repository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(specialities);

    var result = await _handler.Handle(new GetAllSpecialitiesQuery(), CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().HaveCount(2);
  }

  [Fact]
  public async Task Handle_WhenNoSpecialities_ShouldReturnEmptyList()
  {
    _repository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(Array.Empty<Speciality>());

    var result = await _handler.Handle(new GetAllSpecialitiesQuery(), CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().BeEmpty();
  }
}
