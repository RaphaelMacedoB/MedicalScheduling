using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MedicalScheduling.Application.Common.Pagination;
using MedicalScheduling.Application.Features.Appointments.DTOs;
using MedicalScheduling.Application.Features.Appointments.ScheduleAppointment;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.IntegrationTests.Common;
using MedicalScheduling.Presentation.WebAPI.Constants;

namespace MedicalScheduling.IntegrationTests.Appointments;

[Collection(IntegrationTestCollection.Name)]
public sealed class AppointmentsEndpointsTests : IntegrationTestBase
{
  public AppointmentsEndpointsTests(MedicalSchedulingApiFactory factory) : base(factory) { }

  private async Task<(Guid PatientId, Guid DoctorId)> CreatePatientAndDoctorAsync()
  {
    var speciality = await CreateSpecialityAsync();
    var patient = await CreatePatientAsync();
    var doctor = await CreateDoctorAsync(speciality.Id);
    return (patient.Id, doctor.Id);
  }

  [Fact]
  public async Task Schedule_WithValidData_ShouldReturnCreated()
  {
    var (patientId, doctorId) = await CreatePatientAndDoctorAsync();
    var start = IntegrationTestData.FutureStart();
    var command = new ScheduleAppointmentCommand(patientId, doctorId, start, IntegrationTestData.FutureEnd(start));

    var response = await Client.PostAsJsonAsync(ApiRoutes.Appointments.Base, command);

    response.StatusCode.Should().Be(HttpStatusCode.Created);
    var appointment = await response.ReadAsAsync<AppointmentDto>();
    appointment.PatientId.Should().Be(patientId);
    appointment.DoctorId.Should().Be(doctorId);
    appointment.Status.Should().Be("Scheduled");
  }

  [Fact]
  public async Task GetById_WhenExists_ShouldReturnOk()
  {
    var (patientId, doctorId) = await CreatePatientAndDoctorAsync();
    var start = IntegrationTestData.FutureStart();
    var scheduleResponse = await Client.PostAsJsonAsync(
        ApiRoutes.Appointments.Base,
        new ScheduleAppointmentCommand(patientId, doctorId, start, IntegrationTestData.FutureEnd(start)));
    var scheduled = await scheduleResponse.ReadAsAsync<AppointmentDto>();

    var response = await Client.GetAsync($"{ApiRoutes.Appointments.Base}/{scheduled.Id}");

    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var appointment = await response.ReadAsAsync<AppointmentDto>();
    appointment.Id.Should().Be(scheduled.Id);
  }

  [Fact]
  public async Task GetByDoctor_ShouldReturnAppointments()
  {
    var (patientId, doctorId) = await CreatePatientAndDoctorAsync();
    var start = IntegrationTestData.FutureStart();
    await Client.PostAsJsonAsync(
        ApiRoutes.Appointments.Base,
        new ScheduleAppointmentCommand(patientId, doctorId, start, IntegrationTestData.FutureEnd(start)));

    var response = await Client.GetAsync($"{ApiRoutes.Appointments.Base}/doctor/{doctorId}");

    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var page = await response.ReadAsAsync<PagedResponse<AppointmentDto>>();
    page.Items.Should().NotBeEmpty();
  }

  [Fact]
  public async Task GetByPatient_ShouldReturnAppointments()
  {
    var (patientId, doctorId) = await CreatePatientAndDoctorAsync();
    var start = IntegrationTestData.FutureStart();
    await Client.PostAsJsonAsync(
        ApiRoutes.Appointments.Base,
        new ScheduleAppointmentCommand(patientId, doctorId, start, IntegrationTestData.FutureEnd(start)));

    var response = await Client.GetAsync($"{ApiRoutes.Appointments.Base}/patient/{patientId}");

    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var page = await response.ReadAsAsync<PagedResponse<AppointmentDto>>();
    page.Items.Should().NotBeEmpty();
  }

  [Fact]
  public async Task Confirm_WhenScheduled_ShouldReturnNoContent()
  {
    var appointment = await ScheduleAppointmentAsync();

    var response = await Client.PatchAsync(
        $"{ApiRoutes.Appointments.Base}/{appointment.Id}/confirm",
        null);

    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
  }

  [Fact]
  public async Task Complete_WhenConfirmed_ShouldReturnNoContent()
  {
    var appointment = await ScheduleAppointmentAsync();
    await Client.PatchAsync($"{ApiRoutes.Appointments.Base}/{appointment.Id}/confirm", null);

    var content = new StringContent("\"Completed notes\"", Encoding.UTF8, "application/json");
    var response = await Client.PatchAsync(
        $"{ApiRoutes.Appointments.Base}/{appointment.Id}/complete",
        content);

    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
  }

  [Fact]
  public async Task Cancel_WhenScheduled_ShouldReturnNoContent()
  {
    var appointment = await ScheduleAppointmentAsync();

    var response = await Client.PatchAsync(
        $"{ApiRoutes.Appointments.Base}/{appointment.Id}/cancel",
        null);

    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
  }

  [Fact]
  public async Task Schedule_WithConflict_ShouldReturnBadRequest()
  {
    var (patientId, doctorId) = await CreatePatientAndDoctorAsync();
    var start = IntegrationTestData.FutureStart();
    var end = IntegrationTestData.FutureEnd(start);
    var command = new ScheduleAppointmentCommand(patientId, doctorId, start, end);

    await Client.PostAsJsonAsync(ApiRoutes.Appointments.Base, command);
    var response = await Client.PostAsJsonAsync(ApiRoutes.Appointments.Base, command);

    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    var error = await response.ReadErrorAsync();
    error.Code.Should().Be("Appointment.SlotUnavailable");
  }

  [Fact]
  public async Task Schedule_WithUnknownPatient_ShouldReturnNotFound()
  {
    var speciality = await CreateSpecialityAsync();
    var doctor = await CreateDoctorAsync(speciality.Id);
    var start = IntegrationTestData.FutureStart();
    var command = new ScheduleAppointmentCommand(
        Guid.NewGuid(),
        doctor.Id,
        start,
        IntegrationTestData.FutureEnd(start));

    var response = await Client.PostAsJsonAsync(ApiRoutes.Appointments.Base, command);

    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  private async Task<AppointmentDto> ScheduleAppointmentAsync()
  {
    var (patientId, doctorId) = await CreatePatientAndDoctorAsync();
    var start = IntegrationTestData.FutureStart().AddHours(Random.Shared.Next(1, 100));
    var response = await Client.PostAsJsonAsync(
        ApiRoutes.Appointments.Base,
        new ScheduleAppointmentCommand(patientId, doctorId, start, IntegrationTestData.FutureEnd(start)));

    response.EnsureSuccessStatusCode();
    return await response.ReadAsAsync<AppointmentDto>();
  }

}
