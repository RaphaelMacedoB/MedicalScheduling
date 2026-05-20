using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.ValueObjects;

namespace MedicalScheduling.Domain;

public sealed class Doctor : Entity
{
  public string Name { get; private set; } = null!;
  public string Crm { get; private set; } = null!;
  public Email Email { get; private set; } = null!;
  public PhoneNumber Phone { get; private set; } = null!;
  public Guid SpecialtyId { get; private set; }
  public Speciality Speciality { get; private set; } = null!;
  public bool IsActive { get; private set; }

  private readonly List<Appointment> _appointments = [];
  public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

  private Doctor() { } // EF Core

  private Doctor(string name, string crm, Email email, PhoneNumber phone, Guid specialtyId)
  {
    Name = name;
    Crm = crm;
    Email = email;
    Phone = phone;
    SpecialtyId = specialtyId;
    IsActive = true;
  }

  public static Result<Doctor> Create(
      string name,
      string crm,
      string email,
      string phone,
      Guid specialtyId)
  {
    if (string.IsNullOrWhiteSpace(name))
      return Result.Failure<Doctor>(new Error("Doctor.InvalidName", "Nome inválido"));

    if (string.IsNullOrWhiteSpace(crm))
      return Result.Failure<Doctor>(DomainErrors.Doctor.InvalidCrm);

    var emailResult = Email.Create(email);
    if (emailResult.IsFailure) return Result.Failure<Doctor>(emailResult.Error);

    var phoneResult = PhoneNumber.Create(phone);
    if (phoneResult.IsFailure) return Result.Failure<Doctor>(phoneResult.Error);

    return Result.Success(new Doctor(name.Trim(), crm.Trim(), emailResult.Value, phoneResult.Value, specialtyId));
  }

  public void Deactivate()
  {
    IsActive = false;
    UpdatedAt = DateTime.UtcNow;
  }
}