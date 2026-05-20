namespace MedicalScheduling.Infrastructure.Caching;

public sealed class RedisCacheOptions
{
  public const string SectionName = "Cache";

  public string InstanceName { get; init; } = "medicalscheduling:";

  public int DefaultExpirationMinutes { get; init; } = 5;
}
