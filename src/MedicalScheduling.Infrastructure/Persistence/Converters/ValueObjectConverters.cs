using MedicalScheduling.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedicalScheduling.Infrastructure.Persistence.Converters;

internal static class ValueObjectConverters
{
  public static ValueConverter<Cpf, string> CpfConverter { get; } = new(
      cpf => cpf.Value,
      value => Cpf.FromPersistence(value));

  public static ValueConverter<Email, string> EmailConverter { get; } = new(
      email => email.Value,
      value => Email.FromPersistence(value));

  public static ValueConverter<PhoneNumber, string> PhoneConverter { get; } = new(
      phone => phone.Value,
      value => PhoneNumber.FromPersistence(value));
}
