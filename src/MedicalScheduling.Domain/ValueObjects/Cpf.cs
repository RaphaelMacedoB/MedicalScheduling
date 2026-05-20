using System.Text.RegularExpressions;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Domain.ValueObjects;

public sealed class Cpf : ValueObject
{
  public string Value { get; }

  private Cpf(string value) => Value = value;

  public static Result<Cpf> Create(string cpf)
  {
    var digits = Regex.Replace(cpf, @"\D", "");

    if (!IsValid(digits))
      return Result.Failure<Cpf>(DomainErrors.Patient.InvalidCpf);

    return Result.Success(new Cpf(digits));
  }

  internal static Cpf FromPersistence(string value) => new(value);

  private static bool IsValid(string cpf)
  {
    if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
      return false;

    var sum = 0;
    for (var i = 0; i < 9; i++)
      sum += int.Parse(cpf[i].ToString()) * (10 - i);

    var remainder = sum % 11;
    var firstDigit = remainder < 2 ? 0 : 11 - remainder;

    if (int.Parse(cpf[9].ToString()) != firstDigit)
      return false;

    sum = 0;
    for (var i = 0; i < 10; i++)
      sum += int.Parse(cpf[i].ToString()) * (11 - i);

    remainder = sum % 11;
    var secondDigit = remainder < 2 ? 0 : 11 - remainder;

    return int.Parse(cpf[10].ToString()) == secondDigit;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}
