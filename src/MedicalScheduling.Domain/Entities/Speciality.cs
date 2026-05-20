namespace MedicalScheduling.Domain;

using MedicalScheduling.Domain.Primitives;

public sealed class Speciality : Entity
{
  public string Name { get; private set; } = null!;
  public string? Description { get; private set; }

  private readonly List<Doctor> _doctors = [];
  public IReadOnlyCollection<Doctor> Doctors => _doctors.AsReadOnly();

  private Speciality() { } // EF Core

  private Speciality(string name, string? description)
  {
    Name = name;
    Description = description;
  }

  public static Result<Speciality> Create(string name, string? description = null)
  {
    if (string.IsNullOrWhiteSpace(name))
      return Result.Failure<Speciality>(DomainErrors.Speciality.DuplicateName);

    return Result.Success(new Speciality(name.Trim(), description?.Trim()));
  }

  public void Update(string name, string? description)
  {
    Name = name.Trim();
    Description = description?.Trim();
    UpdatedAt = DateTime.UtcNow;
  }
}