using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Patients.Commands.DeactivatePatient;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Patients.Commands.DeactivatePatient;

public sealed class DeactivatePatientHandlerTests
{
  private readonly Mock<IPatientRepository> _repository = new();
  private readonly Mock<IUnitOfWork> _unitOfWork = UnitOfWorkMockExtensions.CreateUnitOfWorkMock();
  private readonly DeactivatePatientHandler _handler;

  public DeactivatePatientHandlerTests()
  {
    _handler = new DeactivatePatientHandler(_repository.Object, _unitOfWork.Object, new NullCacheService());
  }

  [Fact]
  public async Task Handle_WhenPatientExists_ShouldDeactivate()
  {
    var patient = EntityTestHelper.CreatePatient();
    var command = new DeactivatePatientCommand(patient.Id);

    _repository.Setup(r => r.GetByIdAsync(patient.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(patient);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    patient.IsActive.Should().BeFalse();
    _repository.Verify(r => r.Update(patient), Times.Once);
    _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_WhenPatientNotFound_ShouldReturnNotFound()
  {
    var command = new DeactivatePatientCommand(Guid.NewGuid());

    _repository.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Patient?)null);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Patient.NotFound");
    _repository.Verify(r => r.Update(It.IsAny<Patient>()), Times.Never);
  }
}
