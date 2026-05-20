namespace MedicalScheduling.Application.Common.Pagination;

public sealed class PagedResponse<T>
{
  public required IReadOnlyList<T> Items { get; init; }
  public int TotalItems { get; init; }
  public int TotalPages { get; init; }
  public int CurrentPage { get; init; }
  public int PageSize { get; init; }
  public bool HasNextPage { get; init; }
  public bool HasPreviousPage { get; init; }

  public static PagedResponse<T> Create(
      IReadOnlyList<T> items,
      int totalItems,
      int currentPage,
      int pageSize)
  {
    var totalPages = totalItems == 0
        ? 0
        : (int)Math.Ceiling(totalItems / (double)pageSize);

    return new PagedResponse<T>
    {
      Items = items,
      TotalItems = totalItems,
      TotalPages = totalPages,
      CurrentPage = currentPage,
      PageSize = pageSize,
      HasNextPage = currentPage < totalPages,
      HasPreviousPage = currentPage > 1 && totalPages > 0
    };
  }
}
