using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Doctors.Commands.DeactivateDoctor;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Doctors.Commands.DeactivateDoctor;

public sealed class DeactivateDoctorHandlerTests
{
  private readonly Mock<IDoctorRepository> _repository = new();
  private readonly Mock<IUnitOfWork> _unitOfWork = UnitOfWorkMockExtensions.CreateUnitOfWorkMock();
  private readonly DeactivateDoctorHandler _handler;

  public DeactivateDoctorHandlerTests()
  {
    _handler = new DeactivateDoctorHandler(_repository.Object, _unitOfWork.Object);
  }

  [Fact]
  public async Task Handle_WhenDoctorExists_ShouldDeactivate()
  {
    var speciality = EntityTestHelper.CreateSpeciality();
    var doctor = EntityTestHelper.CreateDoctor(speciality);
    var command = new DeactivateDoctorCommand(doctor.Id);

    _repository.Setup(r => r.GetByIdAsync(doctor.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(doctor);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    doctor.IsActive.Should().BeFalse();
    _repository.Verify(r => r.Update(doctor), Times.Once);
    _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_WhenDoctorNotFound_ShouldReturnNotFound()
  {
    var command = new DeactivateDoctorCommand(Guid.NewGuid());

    _repository.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Doctor?)null);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Doctor.NotFound");
  }
}
