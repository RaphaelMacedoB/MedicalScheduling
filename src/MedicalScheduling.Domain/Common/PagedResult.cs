namespace MedicalScheduling.Domain.Common;

public sealed class PagedResult<T>
{
  public required IReadOnlyList<T> Items { get; init; }
  public int TotalCount { get; init; }
}
