using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Enums;

namespace MedicalScheduling.UnitTests.Domain.Entities;

public sealed class AppointmentTests
{
  private static (Guid PatientId, Guid DoctorId, DateTime Start, DateTime End) ValidScheduleData()
  {
    var start = DateTime.UtcNow.AddDays(5);
    return (Guid.NewGuid(), Guid.NewGuid(), start, start.AddHours(1));
  }

  [Fact]
  public void Schedule_WithValidData_ShouldCreateScheduled()
  {
    var (patientId, doctorId, start, end) = ValidScheduleData();

    var result = Appointment.Schedule(patientId, doctorId, start, end);

    result.IsSuccess.Should().BeTrue();
    result.Value.Status.Should().Be(EAppointmentStatus.Scheduled);
  }

  [Fact]
  public void Confirm_WhenScheduled_ShouldSucceed()
  {
    var appointment = Appointment.Schedule(
        ValidScheduleData().PatientId,
        ValidScheduleData().DoctorId,
        ValidScheduleData().Start,
        ValidScheduleData().End).Value;

    var result = appointment.Confirm();

    result.IsSuccess.Should().BeTrue();
    appointment.Status.Should().Be(EAppointmentStatus.Confirmed);
  }

  [Fact]
  public void Cancel_WhenScheduled_ShouldSucceed()
  {
    var appointment = Appointment.Schedule(
        ValidScheduleData().PatientId,
        ValidScheduleData().DoctorId,
        ValidScheduleData().Start,
        ValidScheduleData().End).Value;

    var result = appointment.Cancel();

    result.IsSuccess.Should().BeTrue();
    appointment.Status.Should().Be(EAppointmentStatus.Cancelled);
  }

  [Fact]
  public void Confirm_WhenCancelled_ShouldFail()
  {
    var appointment = Appointment.Schedule(
        ValidScheduleData().PatientId,
        ValidScheduleData().DoctorId,
        ValidScheduleData().Start,
        ValidScheduleData().End).Value;

    appointment.Cancel();
    var result = appointment.Confirm();

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Appointment.AlreadyCancelled");
  }

  [Fact]
  public void Cancel_WhenCompleted_ShouldFail()
  {
    var appointment = Appointment.Schedule(
        ValidScheduleData().PatientId,
        ValidScheduleData().DoctorId,
        ValidScheduleData().Start,
        ValidScheduleData().End).Value;

    appointment.Complete();
    var result = appointment.Cancel();

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Appointment.AlreadyCompleted");
  }
}
