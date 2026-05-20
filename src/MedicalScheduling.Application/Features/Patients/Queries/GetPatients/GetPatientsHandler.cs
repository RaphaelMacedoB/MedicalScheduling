using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Common.Pagination;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Patients.Queries.GetPatients;

public sealed class GetPatientsHandler
    : IRequestHandler<GetPatientsQuery, Result<PagedResponse<PatientDto>>>
{
  private readonly IPatientRepository _repository;
  private readonly IMapper _mapper;

  public GetPatientsHandler(IPatientRepository repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<Result<PagedResponse<PatientDto>>> Handle(GetPatientsQuery request, CancellationToken ct)
  {
    var filter = request.Filter;
    var paged = await _repository.GetPagedAsync(
        filter.Skip,
        filter.Take,
        filter.Search,
        filter.IsActive,
        filter.BirthDateFrom,
        filter.BirthDateTo,
        filter.SortBy,
        filter.SortDescending,
        ct);

    return Result.Success(
        PaginationMapper.ToPagedResponse<Patient, PatientDto>(
            paged,
            _mapper,
            filter.NormalizedPage,
            filter.NormalizedPageSize));
  }
}
