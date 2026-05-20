using MedicalScheduling.Application.Common.Pagination.Filters;
using MedicalScheduling.Application.Features.Doctors.Queries.GetDoctors;
using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Common;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Doctors.Queries.GetDoctors;

public sealed class GetDoctorsHandlerTests
{
  private readonly Mock<IDoctorRepository> _doctorRepository = new();
  private readonly Mock<ISpecialityRepository> _specialityRepository = new();
  private readonly GetDoctorsHandler _handler;

  public GetDoctorsHandlerTests()
  {
    _handler = new GetDoctorsHandler(
        _doctorRepository.Object,
        _specialityRepository.Object,
        MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WhenSpecialityNotFound_ShouldReturnNotFound()
  {
    var specialityId = Guid.NewGuid();
    _specialityRepository.Setup(r => r.GetByIdAsync(specialityId, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Speciality?)null);

    var result = await _handler.Handle(
        new GetDoctorsQuery(new DoctorFilter { SpecialityId = specialityId }),
        CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Speciality.NotFound");
  }

  [Fact]
  public async Task Handle_WhenDoctorsExist_ShouldReturnPagedDtos()
  {
    var speciality = EntityTestHelper.CreateSpeciality();
    var doctor = EntityTestHelper.CreateDoctor(speciality);
    _doctorRepository.Setup(r => r.GetPagedAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<Guid?>(), It.IsAny<bool?>(),
            It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new PagedResult<Doctor> { Items = [doctor], TotalCount = 1 });

    var result = await _handler.Handle(new GetDoctorsQuery(new DoctorFilter()), CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Items.Should().HaveCount(1);
  }
}
