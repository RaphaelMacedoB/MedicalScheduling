using System.Net.Http.Json;
using MedicalScheduling.Application.Features.Patients.Commands.CreatePatient;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Application.Features.Specialities.Commands.CreateSpeciality;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Application.Features.Doctors.Commands.CreateDoctor;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Presentation.Constants;

namespace MedicalScheduling.IntegrationTests.Common;

public abstract class IntegrationTestBase
{
  protected IntegrationTestBase(MedicalSchedulingApiFactory factory)
  {
    Client = factory.CreateAuthenticatedClient();
  }

  protected HttpClient Client { get; }

  protected async Task<SpecialityDto> CreateSpecialityAsync(string? name = null)
  {
    var command = new CreateSpecialityCommand(name ?? IntegrationTestData.UniqueSpecialityName(), "Test description");
    var response = await Client.PostAsJsonAsync(ApiRoutes.Specialities.Base, command);
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadFromJsonAsync<SpecialityDto>()
        ?? throw new InvalidOperationException("Failed to deserialize speciality");
  }

  protected async Task<PatientDto> CreatePatientAsync(string? cpf = null)
  {
    var command = new CreatePatientCommand(
        "Integration Patient",
        IntegrationTestData.UniqueEmail(),
        cpf ?? IntegrationTestData.UniqueCpf(),
        IntegrationTestData.ValidPhone,
        IntegrationTestData.ValidBirthDate);

    var response = await Client.PostAsJsonAsync(ApiRoutes.Patients.Base, command);
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadFromJsonAsync<PatientDto>()
        ?? throw new InvalidOperationException("Failed to deserialize patient");
  }

  protected async Task<DoctorDto> CreateDoctorAsync(Guid specialityId, string? crm = null)
  {
    var command = new CreateDoctorCommand(
        "Integration Doctor",
        crm ?? IntegrationTestData.UniqueCrm(),
        IntegrationTestData.UniqueEmail(),
        IntegrationTestData.ValidPhone,
        specialityId);

    var response = await Client.PostAsJsonAsync(ApiRoutes.Doctors.Base, command);
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadFromJsonAsync<DoctorDto>()
        ?? throw new InvalidOperationException("Failed to deserialize doctor");
  }
}
