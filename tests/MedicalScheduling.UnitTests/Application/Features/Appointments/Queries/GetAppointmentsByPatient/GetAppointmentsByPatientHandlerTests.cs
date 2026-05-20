using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Appointments.GetAppointmentsByPatient;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Appointments.Queries.GetAppointmentsByPatient;

public sealed class GetAppointmentsByPatientHandlerTests
{
  private readonly Mock<IAppointmentRepository> _repository = new();
  private readonly GetAppointmentsByPatientHandler _handler;

  public GetAppointmentsByPatientHandlerTests()
  {
    _handler = new GetAppointmentsByPatientHandler(_repository.Object, MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WhenAppointmentsExist_ShouldReturnMappedDtos()
  {
    var patient = EntityTestHelper.CreatePatient();
    var speciality = EntityTestHelper.CreateSpeciality();
    var doctor = EntityTestHelper.CreateDoctor(speciality);
    var appointment = EntityTestHelper.CreateAppointment(patient, doctor);
    var query = new GetAppointmentsByPatientQuery(patient.Id);

    _repository.Setup(r => r.GetByPatientAsync(patient.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(new[] { appointment });

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().HaveCount(1);
    result.Value.First().PatientId.Should().Be(patient.Id);
  }

  [Fact]
  public async Task Handle_WhenNoAppointments_ShouldReturnEmptyList()
  {
    var query = new GetAppointmentsByPatientQuery(Guid.NewGuid());

    _repository.Setup(r => r.GetByPatientAsync(query.PatientId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(Array.Empty<Appointment>());

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().BeEmpty();
  }
}
