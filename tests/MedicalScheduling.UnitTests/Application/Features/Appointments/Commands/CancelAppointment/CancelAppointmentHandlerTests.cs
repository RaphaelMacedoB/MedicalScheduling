using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Appointments.CancelAppointment;
using MedicalScheduling.Domain.Enums;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Appointments.Commands.CancelAppointment;

public sealed class CancelAppointmentHandlerTests
{
  private readonly Mock<IAppointmentRepository> _repository = new();
  private readonly Mock<IUnitOfWork> _unitOfWork = UnitOfWorkMockExtensions.CreateUnitOfWorkMock();
  private readonly CancelAppointmentHandler _handler;

  public CancelAppointmentHandlerTests()
  {
    _handler = new CancelAppointmentHandler(_repository.Object, _unitOfWork.Object);
  }

  [Fact]
  public async Task Handle_WhenAppointmentExists_ShouldCancel()
  {
    var patient = EntityTestHelper.CreatePatient();
    var speciality = EntityTestHelper.CreateSpeciality();
    var doctor = EntityTestHelper.CreateDoctor(speciality);
    var appointment = EntityTestHelper.CreateAppointment(patient, doctor);
    var command = new CancelAppointmentCommand(appointment.Id);

    _repository.Setup(r => r.GetByIdAsync(appointment.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(appointment);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    appointment.Status.Should().Be(EAppointmentStatus.Cancelled);
    _repository.Verify(r => r.Update(appointment), Times.Once);
    _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_WhenAppointmentNotFound_ShouldReturnNotFound()
  {
    var command = new CancelAppointmentCommand(Guid.NewGuid());

    _repository.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Appointment?)null);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Appointment.NotFound");
  }

  [Fact]
  public async Task Handle_WhenAppointmentIsCompleted_ShouldReturnAlreadyCompleted()
  {
    var patient = EntityTestHelper.CreatePatient();
    var speciality = EntityTestHelper.CreateSpeciality();
    var doctor = EntityTestHelper.CreateDoctor(speciality);
    var appointment = EntityTestHelper.CreateAppointment(patient, doctor);
    appointment.Complete();
    var command = new CancelAppointmentCommand(appointment.Id);

    _repository.Setup(r => r.GetByIdAsync(appointment.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(appointment);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Appointment.AlreadyCompleted");
  }
}
