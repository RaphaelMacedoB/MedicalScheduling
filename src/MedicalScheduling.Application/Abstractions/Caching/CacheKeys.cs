namespace MedicalScheduling.Application.Abstractions.Caching;

public static class CacheKeys
{
  public static class Patients
  {
    public static string ById(Guid id) => $"patients:{id}";
  }

  public static class Doctors
  {
    public static string ById(Guid id) => $"doctors:{id}";
  }
}
