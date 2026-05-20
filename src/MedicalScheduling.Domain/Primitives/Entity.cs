namespace MedicalScheduling.Domain.Primitives;

public abstract class Entity
{
  public Guid Id { get; private init; }
  public DateTime CreatedAt { get; private init; }
  public DateTime? UpdatedAt { get; protected set; }

  protected Entity()
  {
    Id = Guid.NewGuid();
    CreatedAt = DateTime.UtcNow;
  }
}