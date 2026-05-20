using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Doctors.Queries.GetDoctorsBySpeciality;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Doctors.Queries.GetDoctorsBySpeciality;

public sealed class GetDoctorsBySpecialityHandlerTests
{
  private readonly Mock<IDoctorRepository> _doctorRepository = new();
  private readonly Mock<ISpecialityRepository> _specialityRepository = new();
  private readonly GetDoctorsBySpecialityHandler _handler;

  public GetDoctorsBySpecialityHandlerTests()
  {
    _handler = new GetDoctorsBySpecialityHandler(
        _doctorRepository.Object,
        _specialityRepository.Object,
        new NullCacheService(),
        MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WhenSpecialityExists_ShouldReturnDoctors()
  {
    var speciality = EntityTestHelper.CreateSpeciality();
    var doctor = EntityTestHelper.CreateDoctor(speciality, crm: TestData.UniqueCrm());
    var query = new GetDoctorsBySpecialityQuery(speciality.Id);

    _specialityRepository.Setup(r => r.GetByIdAsync(speciality.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(speciality);
    _doctorRepository.Setup(r => r.GetBySpecialityAsync(speciality.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(new[] { doctor });

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().HaveCount(1);
    result.Value.First().SpecialityName.Should().Be(TestData.SpecialityName);
  }

  [Fact]
  public async Task Handle_WhenSpecialityNotFound_ShouldReturnNotFound()
  {
    var query = new GetDoctorsBySpecialityQuery(Guid.NewGuid());

    _specialityRepository.Setup(r => r.GetByIdAsync(query.SpecialityId, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Speciality?)null);

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Speciality.NotFound");
  }
}
