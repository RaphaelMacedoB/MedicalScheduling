using System.Net;
using System.Net.Http.Json;
using MedicalScheduling.Application.Features.Patients.Commands.CreatePatient;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.IntegrationTests.Common;
using MedicalScheduling.Presentation.Constants;

namespace MedicalScheduling.IntegrationTests.Patients;

[Collection(IntegrationTestCollection.Name)]
public sealed class PatientsEndpointsTests : IntegrationTestBase
{
  public PatientsEndpointsTests(MedicalSchedulingApiFactory factory) : base(factory) { }

  [Fact]
  public async Task GetAll_ShouldReturnOk()
  {
    var response = await Client.GetAsync(ApiRoutes.Patients.Base);

    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Create_WithValidData_ShouldReturnCreated()
  {
    var patient = await CreatePatientAsync();

    patient.Name.Should().Be("Integration Patient");
    patient.Cpf.Should().HaveLength(11);
  }

  [Fact]
  public async Task GetById_WhenExists_ShouldReturnOk()
  {
    var created = await CreatePatientAsync();

    var response = await Client.GetAsync($"{ApiRoutes.Patients.Base}/{created.Id}");

    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var patient = await response.ReadAsAsync<PatientDto>();
    patient.Id.Should().Be(created.Id);
  }

  [Fact]
  public async Task GetById_WhenNotFound_ShouldReturnNotFound()
  {
    var response = await Client.GetAsync($"{ApiRoutes.Patients.Base}/{Guid.NewGuid()}");

    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task Create_WithInvalidCpf_ShouldReturnBadRequest()
  {
    var command = new CreatePatientCommand(
        "Test",
        IntegrationTestData.UniqueEmail(),
        "11111111111",
        IntegrationTestData.ValidPhone,
        IntegrationTestData.ValidBirthDate);

    var response = await Client.PostAsJsonAsync(ApiRoutes.Patients.Base, command);

    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Fact]
  public async Task Create_WithInvalidEmail_ShouldReturnUnprocessableEntity()
  {
    var command = new CreatePatientCommand(
        "Test",
        "invalid-email",
        IntegrationTestData.UniqueCpf(),
        IntegrationTestData.ValidPhone,
        IntegrationTestData.ValidBirthDate);

    var response = await Client.PostAsJsonAsync(ApiRoutes.Patients.Base, command);

    response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
  }

  [Fact]
  public async Task Deactivate_WhenExists_ShouldReturnNoContent()
  {
    var created = await CreatePatientAsync();

    var response = await Client.DeleteAsync($"{ApiRoutes.Patients.Base}/{created.Id}");

    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
  }

  [Fact]
  public async Task Deactivate_WhenNotFound_ShouldReturnNotFound()
  {
    var response = await Client.DeleteAsync($"{ApiRoutes.Patients.Base}/{Guid.NewGuid()}");

    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }
}
