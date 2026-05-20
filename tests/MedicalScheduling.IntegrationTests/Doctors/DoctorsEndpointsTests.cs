using System.Net;
using System.Net.Http.Json;
using MedicalScheduling.Application.Common.Pagination;
using MedicalScheduling.Application.Features.Doctors.Commands.CreateDoctor;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.IntegrationTests.Common;
using MedicalScheduling.Presentation.WebAPI.Constants;

namespace MedicalScheduling.IntegrationTests.Doctors;

[Collection(IntegrationTestCollection.Name)]
public sealed class DoctorsEndpointsTests : IntegrationTestBase
{
  public DoctorsEndpointsTests(MedicalSchedulingApiFactory factory) : base(factory) { }

  [Fact]
  public async Task Create_WithValidData_ShouldReturnCreated()
  {
    var speciality = await CreateSpecialityAsync();
    var doctor = await CreateDoctorAsync(speciality.Id);

    doctor.SpecialityId.Should().Be(speciality.Id);
    doctor.Crm.Should().NotBeNullOrEmpty();
  }

  [Fact]
  public async Task GetById_WhenExists_ShouldReturnOk()
  {
    var speciality = await CreateSpecialityAsync();
    var created = await CreateDoctorAsync(speciality.Id);

    var response = await Client.GetAsync($"{ApiRoutes.Doctors.Base}/{created.Id}");

    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var doctor = await response.ReadAsAsync<DoctorDto>();
    doctor.Id.Should().Be(created.Id);
    doctor.SpecialityName.Should().Be(speciality.Name);
  }

  [Fact]
  public async Task GetById_WhenNotFound_ShouldReturnNotFound()
  {
    var response = await Client.GetAsync($"{ApiRoutes.Doctors.Base}/{Guid.NewGuid()}");

    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task GetBySpeciality_WhenExists_ShouldReturnOk()
  {
    var speciality = await CreateSpecialityAsync();
    await CreateDoctorAsync(speciality.Id);

    var response = await Client.GetAsync($"{ApiRoutes.Doctors.Base}/speciality/{speciality.Id}");

    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var page = await response.ReadAsAsync<PagedResponse<DoctorDto>>();
    page.Items.Should().NotBeEmpty();
  }

  [Fact]
  public async Task GetBySpeciality_WhenSpecialityNotFound_ShouldReturnNotFound()
  {
    var response = await Client.GetAsync($"{ApiRoutes.Doctors.Base}/speciality/{Guid.NewGuid()}");

    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task Create_WithUnknownSpeciality_ShouldReturnNotFound()
  {
    var command = new CreateDoctorCommand(
        "Doctor",
        IntegrationTestData.UniqueCrm(),
        IntegrationTestData.UniqueEmail(),
        IntegrationTestData.ValidPhone,
        Guid.NewGuid());

    var response = await Client.PostAsJsonAsync(ApiRoutes.Doctors.Base, command);

    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task Deactivate_WhenExists_ShouldReturnNoContent()
  {
    var speciality = await CreateSpecialityAsync();
    var created = await CreateDoctorAsync(speciality.Id);

    var response = await Client.DeleteAsync($"{ApiRoutes.Doctors.Base}/{created.Id}");

    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
  }
}
