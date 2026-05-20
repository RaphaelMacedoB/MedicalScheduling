using System.Text.RegularExpressions;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
  public string Value { get; }

  private PhoneNumber(string value) => Value = value;

  public static Result<PhoneNumber> Create(string phone)
  {
    var digits = Regex.Replace(phone, @"\D", "");

    if (digits.Length < 10 || digits.Length > 11)
      return Result.Failure<PhoneNumber>(DomainErrors.Patient.InvalidPhone);

    return Result.Success(new PhoneNumber(digits));
  }

  internal static PhoneNumber FromPersistence(string value) => new(value);

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}
