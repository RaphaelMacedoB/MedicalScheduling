namespace MedicalScheduling.IntegrationTests.Common;

internal static class CpfGenerator
{
  public static string GenerateUnique()
  {
    var nineDigits = Random.Shared.Next(100_000_000, 999_999_999).ToString();
    var firstDigit = CalculateDigit(nineDigits, 10);
    var secondDigit = CalculateDigit(nineDigits + firstDigit, 11);
    return nineDigits + firstDigit + secondDigit;
  }

  private static string CalculateDigit(string digits, int multiplierStart)
  {
    var sum = 0;
    for (var i = 0; i < digits.Length; i++)
      sum += (digits[i] - '0') * (multiplierStart - i);

    var remainder = sum % 11;
    var digit = remainder < 2 ? 0 : 11 - remainder;
    return digit.ToString();
  }
}
