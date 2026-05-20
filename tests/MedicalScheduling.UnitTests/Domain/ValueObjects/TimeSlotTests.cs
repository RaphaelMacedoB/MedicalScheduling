using MedicalScheduling.Domain.ValueObjects;

namespace MedicalScheduling.UnitTests.Domain.ValueObjects;

public sealed class TimeSlotTests
{
  [Fact]
  public void Create_WithFutureValidRange_ShouldSucceed()
  {
    var start = DateTime.UtcNow.AddDays(2);
    var end = start.AddHours(1);

    var result = TimeSlot.Create(start, end);

    result.IsSuccess.Should().BeTrue();
    result.Value.Start.Should().Be(start);
    result.Value.End.Should().Be(end);
  }

  [Fact]
  public void Create_WithPastStart_ShouldFail()
  {
    var start = DateTime.UtcNow.AddDays(-1);
    var end = start.AddHours(1);

    var result = TimeSlot.Create(start, end);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Appointment.PastDate");
  }

  [Fact]
  public void Create_WithEndBeforeStart_ShouldFail()
  {
    var start = DateTime.UtcNow.AddDays(2);
    var end = start.AddMinutes(-30);

    var result = TimeSlot.Create(start, end);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Appointment.InvalidTimeSlot");
  }

  [Fact]
  public void OverlapsWith_WhenRangesIntersect_ShouldReturnTrue()
  {
    var slot1 = TimeSlot.Create(
        DateTime.UtcNow.AddDays(3),
        DateTime.UtcNow.AddDays(3).AddHours(2)).Value;

    var slot2 = TimeSlot.Create(
        DateTime.UtcNow.AddDays(3).AddHours(1),
        DateTime.UtcNow.AddDays(3).AddHours(3)).Value;

    slot1.OverlapsWith(slot2).Should().BeTrue();
  }
}
