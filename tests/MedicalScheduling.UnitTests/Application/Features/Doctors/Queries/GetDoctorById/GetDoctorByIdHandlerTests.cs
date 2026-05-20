using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Doctors.Queries.GetDoctorById;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Doctors.Queries.GetDoctorById;

public sealed class GetDoctorByIdHandlerTests
{
  private readonly Mock<IDoctorRepository> _repository = new();
  private readonly GetDoctorByIdHandler _handler;

  public GetDoctorByIdHandlerTests()
  {
    _handler = new GetDoctorByIdHandler(_repository.Object, new NullCacheService(), MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WhenDoctorExists_ShouldReturnDto()
  {
    var speciality = EntityTestHelper.CreateSpeciality();
    var doctor = EntityTestHelper.CreateDoctor(speciality);
    var query = new GetDoctorByIdQuery(doctor.Id);

    _repository.Setup(r => r.GetByIdAsync(doctor.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(doctor);

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Id.Should().Be(doctor.Id);
    result.Value.SpecialityName.Should().Be(TestData.SpecialityName);
  }

  [Fact]
  public async Task Handle_WhenDoctorNotFound_ShouldReturnNotFound()
  {
    var query = new GetDoctorByIdQuery(Guid.NewGuid());

    _repository.Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Doctor?)null);

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Doctor.NotFound");
  }
}
