using MedicalScheduling.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace MedicalScheduling.Infrastructure.Persistence;

internal static class QueryablePaginationExtensions
{
  public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
      this IQueryable<T> query,
      int skip,
      int take,
      CancellationToken cancellationToken = default)
  {
    var totalCount = await query.CountAsync(cancellationToken);
    var items = await query
        .Skip(skip)
        .Take(take)
        .ToListAsync(cancellationToken);

    return new PagedResult<T>
    {
      Items = items,
      TotalCount = totalCount
    };
  }
}
