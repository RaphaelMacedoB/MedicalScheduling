using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Appointments.GetAppointmentById;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Appointments.Queries.GetAppointmentById;

public sealed class GetAppointmentByIdHandlerTests
{
  private readonly Mock<IAppointmentRepository> _repository = new();
  private readonly GetAppointmentByIdHandler _handler;

  public GetAppointmentByIdHandlerTests()
  {
    _handler = new GetAppointmentByIdHandler(_repository.Object, MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WhenAppointmentExists_ShouldReturnDto()
  {
    var patient = EntityTestHelper.CreatePatient();
    var speciality = EntityTestHelper.CreateSpeciality();
    var doctor = EntityTestHelper.CreateDoctor(speciality);
    var appointment = EntityTestHelper.CreateAppointment(patient, doctor);
    var query = new GetAppointmentByIdQuery(appointment.Id);

    _repository.Setup(r => r.GetByIdAsync(appointment.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(appointment);

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Id.Should().Be(appointment.Id);
    result.Value.PatientName.Should().Be(TestData.PatientName);
    result.Value.DoctorName.Should().Be(TestData.DoctorName);
  }

  [Fact]
  public async Task Handle_WhenAppointmentNotFound_ShouldReturnNotFound()
  {
    var query = new GetAppointmentByIdQuery(Guid.NewGuid());

    _repository.Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Appointment?)null);

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Appointment.NotFound");
  }
}
