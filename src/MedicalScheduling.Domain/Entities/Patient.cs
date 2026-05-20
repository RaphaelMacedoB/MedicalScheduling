namespace MedicalScheduling.Domain;

using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.ValueObjects;

public sealed class Patient : Entity
{
  public string Name { get; private set; } = null!;
  public Email Email { get; private set; } = null!;
  public Cpf Cpf { get; private set; } = null!;
  public PhoneNumber Phone { get; private set; } = null!;
  public DateOnly BirthDate { get; private set; }
  public bool IsActive { get; private set; }

  private readonly List<Appointment> _appointments = [];
  public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

  private Patient() { } // EF Core

  private Patient(string name, Email email, Cpf cpf, PhoneNumber phone, DateOnly birthDate)
  {
    Name = name;
    Email = email;
    Cpf = cpf;
    Phone = phone;
    BirthDate = birthDate;
    IsActive = true;
  }

  public static Result<Patient> Create(
      string name,
      string email,
      string cpf,
      string phone,
      DateOnly birthDate)
  {
    if (string.IsNullOrWhiteSpace(name))
      return Result.Failure<Patient>(new Error("Patient.InvalidName", "Nome inválido"));

    var emailResult = Email.Create(email);
    if (emailResult.IsFailure) return Result.Failure<Patient>(emailResult.Error);

    var cpfResult = Cpf.Create(cpf);
    if (cpfResult.IsFailure) return Result.Failure<Patient>(cpfResult.Error);

    var phoneResult = PhoneNumber.Create(phone);
    if (phoneResult.IsFailure) return Result.Failure<Patient>(phoneResult.Error);

    return Result.Success(new Patient(name.Trim(), emailResult.Value, cpfResult.Value, phoneResult.Value, birthDate));
  }

  public void Deactivate()
  {
    IsActive = false;
    UpdatedAt = DateTime.UtcNow;
  }
}