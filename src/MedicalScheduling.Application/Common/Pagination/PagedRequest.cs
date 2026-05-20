namespace MedicalScheduling.Application.Common.Pagination;

public abstract record PagedRequest
{
  public const int DefaultPage = 1;
  public const int DefaultPageSize = 10;
  public const int MaxPageSize = 100;

  public int Page { get; init; } = DefaultPage;
  public int PageSize { get; init; } = DefaultPageSize;

  public int NormalizedPage => Page < DefaultPage ? DefaultPage : Page;

  public int NormalizedPageSize => PageSize switch
  {
    < 1 => DefaultPageSize,
    > MaxPageSize => MaxPageSize,
    _ => PageSize
  };

  public int Skip => (NormalizedPage - 1) * NormalizedPageSize;
  public int Take => NormalizedPageSize;
}
