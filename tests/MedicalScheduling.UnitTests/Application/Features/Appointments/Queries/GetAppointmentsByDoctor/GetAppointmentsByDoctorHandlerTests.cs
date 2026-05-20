using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Appointments.GetAppointmentsByDoctor;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Appointments.Queries.GetAppointmentsByDoctor;

public sealed class GetAppointmentsByDoctorHandlerTests
{
  private readonly Mock<IAppointmentRepository> _repository = new();
  private readonly GetAppointmentsByDoctorHandler _handler;

  public GetAppointmentsByDoctorHandlerTests()
  {
    _handler = new GetAppointmentsByDoctorHandler(_repository.Object, MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WhenAppointmentsExist_ShouldReturnMappedDtos()
  {
    var patient = EntityTestHelper.CreatePatient();
    var speciality = EntityTestHelper.CreateSpeciality();
    var doctor = EntityTestHelper.CreateDoctor(speciality);
    var appointment = EntityTestHelper.CreateAppointment(patient, doctor);
    var query = new GetAppointmentsByDoctorQuery(doctor.Id);

    _repository.Setup(r => r.GetByDoctorAsync(doctor.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(new[] { appointment });

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().HaveCount(1);
    result.Value.First().DoctorId.Should().Be(doctor.Id);
  }

  [Fact]
  public async Task Handle_WhenNoAppointments_ShouldReturnEmptyList()
  {
    var query = new GetAppointmentsByDoctorQuery(Guid.NewGuid());

    _repository.Setup(r => r.GetByDoctorAsync(query.DoctorId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(Array.Empty<Appointment>());

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Should().BeEmpty();
  }
}
