namespace MedicalScheduling.Application.Abstractions.Caching;

public static class CacheKeys
{
  public static class Specialities
  {
    public const string All = "specialities:all";
  }

  public static class Patients
  {
    public const string All = "patients:all";

    public static string ById(Guid id) => $"patients:{id}";
  }

  public static class Doctors
  {
    public static string ById(Guid id) => $"doctors:{id}";

    public static string BySpeciality(Guid specialityId) => $"doctors:speciality:{specialityId}";
  }
}
