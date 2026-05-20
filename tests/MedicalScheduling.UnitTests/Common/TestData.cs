namespace MedicalScheduling.UnitTests.Common;

public static class TestData
{
  public const string ValidCpf = "52998224725";
  public const string ValidCpfFormatted = "529.982.247-25";
  public const string InvalidCpf = "11111111111";

  public const string ValidEmail = "joao.silva@email.com";
  public const string InvalidEmail = "not-an-email";

  public const string ValidPhone = "11987654321";
  public const string ValidPhoneFormatted = "(11) 98765-4321";
  public const string InvalidPhone = "12345";

  public const string PatientName = "Maria Silva";
  public const string DoctorName = "Dr. João Santos";
  public const string Crm = "CRM123456";
  public const string SpecialityName = "Cardiologia";

  public static DateOnly ValidBirthDate => DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30));

  public static DateTime FutureStart => DateTime.UtcNow.AddDays(7).Date.AddHours(10);

  public static DateTime FutureEnd => FutureStart.AddHours(1);

  public static string UniqueCpf()
  {
    var nineDigits = Random.Shared.Next(100_000_000, 999_999_999).ToString();
    var d1 = CpfCheckDigit(nineDigits, 10);
    var d2 = CpfCheckDigit(nineDigits + d1, 11);
    return nineDigits + d1 + d2;
  }

  private static string CpfCheckDigit(string digits, int start)
  {
    var sum = digits.Select((t, i) => (t - '0') * (start - i)).Sum();
    var r = sum % 11;
    return (r < 2 ? 0 : 11 - r).ToString();
  }

  public static string UniqueCrm() => $"CRM{Random.Shared.Next(100000, 999999)}";

  public static string UniqueEmail() => $"user{Guid.NewGuid():N}@email.com";
}
