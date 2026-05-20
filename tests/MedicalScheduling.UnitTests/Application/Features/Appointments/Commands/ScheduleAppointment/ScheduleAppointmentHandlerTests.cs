using MedicalScheduling.Domain;
using AutoMapper;
using MedicalScheduling.Application.Features.Appointments.ScheduleAppointment;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Appointments.Commands.ScheduleAppointment;

public sealed class ScheduleAppointmentHandlerTests
{
  private readonly Mock<IAppointmentRepository> _appointmentRepository = new();
  private readonly Mock<IPatientRepository> _patientRepository = new();
  private readonly Mock<IDoctorRepository> _doctorRepository = new();
  private readonly Mock<IUnitOfWork> _unitOfWork = UnitOfWorkMockExtensions.CreateUnitOfWorkMock();
  private readonly Mock<IMapper> _mapper = new();

  private ScheduleAppointmentHandler CreateHandler() => new(
      _appointmentRepository.Object,
      _patientRepository.Object,
      _doctorRepository.Object,
      _unitOfWork.Object,
      _mapper.Object);

  [Fact]
  public async Task Handle_WithValidCommand_ShouldScheduleAppointment()
  {
    var patient = EntityTestHelper.CreatePatient();
    var speciality = EntityTestHelper.CreateSpeciality();
    var doctor = EntityTestHelper.CreateDoctor(speciality);
    var command = new ScheduleAppointmentCommand(
        patient.Id,
        doctor.Id,
        TestData.FutureStart,
        TestData.FutureEnd);

    _patientRepository.Setup(r => r.GetByIdAsync(patient.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(patient);
    _doctorRepository.Setup(r => r.GetByIdAsync(doctor.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(doctor);
    _appointmentRepository.Setup(r => r.HasConflictAsync(
            doctor.Id, command.Start, command.End, It.IsAny<CancellationToken>()))
        .ReturnsAsync(false);
    _mapper.Setup(m => m.Map<AppointmentDto>(It.IsAny<Appointment>()))
        .Returns((Appointment a) => new AppointmentDto(
            a.Id, a.PatientId, patient.Name, a.DoctorId, doctor.Name,
            speciality.Name, TestData.FutureStart, TestData.FutureEnd,
            "Scheduled", null, a.CreatedAt));

    var result = await CreateHandler().Handle(command, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.PatientId.Should().Be(patient.Id);
    _appointmentRepository.Verify(r => r.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()), Times.Once);
    _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_WhenPatientNotFound_ShouldReturnNotFound()
  {
    var command = new ScheduleAppointmentCommand(
        Guid.NewGuid(),
        Guid.NewGuid(),
        TestData.FutureStart,
        TestData.FutureEnd);

    _patientRepository.Setup(r => r.GetByIdAsync(command.PatientId, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Patient?)null);

    var result = await CreateHandler().Handle(command, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Patient.NotFound");
  }

  [Fact]
  public async Task Handle_WhenDoctorNotFound_ShouldReturnNotFound()
  {
    var patient = EntityTestHelper.CreatePatient();
    var command = new ScheduleAppointmentCommand(
        patient.Id,
        Guid.NewGuid(),
        TestData.FutureStart,
        TestData.FutureEnd);

    _patientRepository.Setup(r => r.GetByIdAsync(patient.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(patient);
    _doctorRepository.Setup(r => r.GetByIdAsync(command.DoctorId, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Doctor?)null);

    var result = await CreateHandler().Handle(command, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Doctor.NotFound");
  }

  [Fact]
  public async Task Handle_WhenSlotHasConflict_ShouldReturnSlotUnavailable()
  {
    var patient = EntityTestHelper.CreatePatient();
    var speciality = EntityTestHelper.CreateSpeciality();
    var doctor = EntityTestHelper.CreateDoctor(speciality);
    var command = new ScheduleAppointmentCommand(
        patient.Id,
        doctor.Id,
        TestData.FutureStart,
        TestData.FutureEnd);

    _patientRepository.Setup(r => r.GetByIdAsync(patient.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(patient);
    _doctorRepository.Setup(r => r.GetByIdAsync(doctor.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(doctor);
    _appointmentRepository.Setup(r => r.HasConflictAsync(
            doctor.Id, command.Start, command.End, It.IsAny<CancellationToken>()))
        .ReturnsAsync(true);

    var result = await CreateHandler().Handle(command, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Appointment.SlotUnavailable");
  }
}
