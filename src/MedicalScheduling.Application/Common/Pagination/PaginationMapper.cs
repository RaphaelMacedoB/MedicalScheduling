using AutoMapper;
using MedicalScheduling.Domain.Common;

namespace MedicalScheduling.Application.Common.Pagination;

public static class PaginationMapper
{
  public static PagedResponse<TDestination> ToPagedResponse<TSource, TDestination>(
      PagedResult<TSource> pagedResult,
      IMapper mapper,
      int currentPage,
      int pageSize)
  {
    var items = mapper.Map<List<TDestination>>(pagedResult.Items);
    return PagedResponse<TDestination>.Create(items, pagedResult.TotalCount, currentPage, pageSize);
  }
}
