namespace MedicalScheduling.Domain.ValueObjects;

using System.Text.RegularExpressions;
using MedicalScheduling.Domain.Primitives;

public sealed class Email : ValueObject
{
  public string Value { get; }

  private Email(string value) => Value = value;

  public static Result<Email> Create(string email)
  {
    if (string.IsNullOrWhiteSpace(email))
      return Result.Failure<Email>(DomainErrors.Patient.InvalidEmail);

    if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
      return Result.Failure<Email>(DomainErrors.Patient.InvalidEmail);

    return Result.Success(new Email(email.ToLowerInvariant()));
  }

  internal static Email FromPersistence(string value) => new(value.ToLowerInvariant());

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}
