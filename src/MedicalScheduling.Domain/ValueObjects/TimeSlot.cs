using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Domain.ValueObjects;

public sealed class TimeSlot : ValueObject
{
  public DateTime Start { get; private set; }
  public DateTime End { get; private set; }

  private TimeSlot() { }

  private TimeSlot(DateTime start, DateTime end)
  {
    Start = start;
    End = end;
  }

  public static Result<TimeSlot> Create(DateTime start, DateTime end)
  {
    if (start <= DateTime.UtcNow)
      return Result.Failure<TimeSlot>(DomainErrors.Appointment.PastDate);

    if (end <= start)
      return Result.Failure<TimeSlot>(DomainErrors.Appointment.InvalidTimeSlot);

    return Result.Success(new TimeSlot(start, end));
  }

  internal static TimeSlot FromPersistence(DateTime start, DateTime end) => new(start, end);

  public bool OverlapsWith(TimeSlot other) => Start < other.End && End > other.Start;

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Start;
    yield return End;
  }
}
