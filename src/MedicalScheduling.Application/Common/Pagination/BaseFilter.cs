namespace MedicalScheduling.Application.Common.Pagination;

public abstract record BaseFilter : PagedRequest
{
  public string? Search { get; init; }
  public string? SortBy { get; init; }
  public SortDirection SortDirection { get; init; } = SortDirection.Asc;

  public bool SortDescending => SortDirection == SortDirection.Desc;
}
