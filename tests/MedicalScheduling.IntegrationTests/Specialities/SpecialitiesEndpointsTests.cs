using System.Net;
using System.Net.Http.Json;
using MedicalScheduling.Application.Common.Pagination;
using MedicalScheduling.Application.Features.Specialities.Commands.CreateSpeciality;
using MedicalScheduling.Application.Features.Specialities.Commands.UpdateSpeciality;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.IntegrationTests.Common;
using MedicalScheduling.Presentation.WebAPI.Constants;

namespace MedicalScheduling.IntegrationTests.Specialities;

[Collection(IntegrationTestCollection.Name)]
public sealed class SpecialitiesEndpointsTests : IntegrationTestBase
{
  public SpecialitiesEndpointsTests(MedicalSchedulingApiFactory factory) : base(factory) { }

  [Fact]
  public async Task GetSpecialities_ShouldReturnPagedResult()
  {
    var response = await Client.GetAsync($"{ApiRoutes.Specialities.Base}?page=1&pageSize=10");

    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var page = await response.ReadAsAsync<PagedResponse<SpecialityDto>>();
    page.Items.Should().NotBeNull();
    page.CurrentPage.Should().Be(1);
  }

  [Fact]
  public async Task Create_WithValidData_ShouldReturnCreated()
  {
    var command = new CreateSpecialityCommand(IntegrationTestData.UniqueSpecialityName(), "Description");

    var response = await Client.PostAsJsonAsync(ApiRoutes.Specialities.Base, command);

    response.StatusCode.Should().Be(HttpStatusCode.Created);
    var dto = await response.ReadAsAsync<SpecialityDto>();
    dto.Name.Should().Be(command.Name);
  }

  [Fact]
  public async Task Create_WithEmptyName_ShouldReturnUnprocessableEntity()
  {
    var command = new CreateSpecialityCommand("", null);

    var response = await Client.PostAsJsonAsync(ApiRoutes.Specialities.Base, command);

    response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
  }

  [Fact]
  public async Task Create_WithDuplicateName_ShouldReturnBadRequest()
  {
    var name = IntegrationTestData.UniqueSpecialityName();
    await CreateSpecialityAsync(name);

    var response = await Client.PostAsJsonAsync(
        ApiRoutes.Specialities.Base,
        new CreateSpecialityCommand(name, null));

    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    var error = await response.ReadErrorAsync();
    error.Code.Should().Be("Speciality.DuplicateName");
  }

  [Fact]
  public async Task Update_WithValidData_ShouldReturnOk()
  {
    var created = await CreateSpecialityAsync();
    var command = new UpdateSpecialityCommand(created.Id, "Updated Name", "Updated desc");

    var response = await Client.PutAsJsonAsync(
        $"{ApiRoutes.Specialities.Base}/{created.Id}",
        command);

    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var dto = await response.ReadAsAsync<SpecialityDto>();
    dto.Name.Should().Be("Updated Name");
  }

  [Fact]
  public async Task Update_WhenNotFound_ShouldReturnNotFound()
  {
    var command = new UpdateSpecialityCommand(Guid.NewGuid(), "Name", null);

    var response = await Client.PutAsJsonAsync(
        $"{ApiRoutes.Specialities.Base}/{command.Id}",
        command);

    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }
}
