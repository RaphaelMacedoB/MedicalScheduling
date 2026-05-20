using System.Net.Http.Json;
using System.Text.Json;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.IntegrationTests.Common;

public static class HttpResponseExtensions
{
  private static readonly JsonSerializerOptions JsonOptions = new()
  {
    PropertyNameCaseInsensitive = true
  };

  public static async Task<T> ReadAsAsync<T>(this HttpResponseMessage response)
  {
    var content = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<T>(content, JsonOptions)
        ?? throw new InvalidOperationException($"Failed to deserialize {typeof(T).Name}");
  }

  public static async Task<Error> ReadErrorAsync(this HttpResponseMessage response) =>
      await response.ReadAsAsync<Error>();
}
