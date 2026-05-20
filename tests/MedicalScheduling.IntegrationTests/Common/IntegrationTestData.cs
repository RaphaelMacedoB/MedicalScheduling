namespace MedicalScheduling.IntegrationTests.Common;

public static class IntegrationTestData
{
  public const string ValidCpf = "52998224725";
  public const string ValidEmail = "integration@email.com";
  public const string ValidPhone = "11987654321";

  public static string UniqueCpf() => CpfGenerator.GenerateUnique();

  public static string UniqueCrm() => $"CRM{Random.Shared.Next(100000, 999999)}";

  public static string UniqueEmail() => $"user{Guid.NewGuid():N}@test.com";

  public static string UniqueSpecialityName() => $"Speciality-{Guid.NewGuid():N}";

  public static DateOnly ValidBirthDate => DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25));

  public static DateTime FutureStart() => DateTime.UtcNow.AddDays(10).Date.AddHours(9);

  public static DateTime FutureEnd(DateTime start) => start.AddHours(1);
}
