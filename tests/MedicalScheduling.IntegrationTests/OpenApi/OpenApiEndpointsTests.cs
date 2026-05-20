using System.Net;
using MedicalScheduling.IntegrationTests.Common;
using MedicalScheduling.Presentation.WebAPI.Constants;

namespace MedicalScheduling.IntegrationTests.OpenApi;

[Collection(IntegrationTestCollection.Name)]
public sealed class OpenApiEndpointsTests
{
  private readonly HttpClient _client;

  public OpenApiEndpointsTests(MedicalSchedulingApiFactory factory) =>
      _client = factory.CreateAuthenticatedClient();

  [Theory]
  [InlineData("GET", "/swagger/v1/swagger.json")]
  public async Task SwaggerDocument_ShouldBeAvailable(string method, string path)
  {
    var request = new HttpRequestMessage(new HttpMethod(method), path);
    var response = await _client.SendAsync(request);

    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var content = await response.Content.ReadAsStringAsync();
    content.Should().Contain("Medical Scheduling API");
    content.Should().Contain(ApiRoutes.Patients.Base);
    content.Should().Contain(ApiRoutes.Doctors.Base);
    content.Should().Contain(ApiRoutes.Appointments.Base);
    content.Should().Contain(ApiRoutes.Specialities.Base);
  }
}
