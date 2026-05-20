using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Common.Pagination;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Specialities.Queries.GetSpecialities;

public sealed class GetSpecialitiesHandler
    : IRequestHandler<GetSpecialitiesQuery, Result<PagedResponse<SpecialityDto>>>
{
  private readonly ISpecialityRepository _repository;
  private readonly IMapper _mapper;

  public GetSpecialitiesHandler(ISpecialityRepository repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<Result<PagedResponse<SpecialityDto>>> Handle(GetSpecialitiesQuery request, CancellationToken ct)
  {
    var filter = request.Filter;
    var paged = await _repository.GetPagedAsync(
        filter.Skip,
        filter.Take,
        filter.Search,
        filter.SortBy,
        filter.SortDescending,
        ct);

    return Result.Success(
        PaginationMapper.ToPagedResponse<Speciality, SpecialityDto>(
            paged,
            _mapper,
            filter.NormalizedPage,
            filter.NormalizedPageSize));
  }
}
