using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Common.Pagination;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Doctors.Queries.GetDoctors;

public sealed class GetDoctorsHandler
    : IRequestHandler<GetDoctorsQuery, Result<PagedResponse<DoctorDto>>>
{
  private readonly IDoctorRepository _doctorRepository;
  private readonly ISpecialityRepository _specialityRepository;
  private readonly IMapper _mapper;

  public GetDoctorsHandler(
      IDoctorRepository doctorRepository,
      ISpecialityRepository specialityRepository,
      IMapper mapper)
  {
    _doctorRepository = doctorRepository;
    _specialityRepository = specialityRepository;
    _mapper = mapper;
  }

  public async Task<Result<PagedResponse<DoctorDto>>> Handle(GetDoctorsQuery request, CancellationToken ct)
  {
    var filter = request.Filter;

    if (filter.SpecialityId.HasValue)
    {
      var speciality = await _specialityRepository.GetByIdAsync(filter.SpecialityId.Value, ct);
      if (speciality is null)
        return Result.Failure<PagedResponse<DoctorDto>>(DomainErrors.Speciality.NotFound);
    }

    var paged = await _doctorRepository.GetPagedAsync(
        filter.Skip,
        filter.Take,
        filter.Search,
        filter.SpecialityId,
        filter.IsActive,
        filter.SortBy,
        filter.SortDescending,
        ct);

    return Result.Success(
        PaginationMapper.ToPagedResponse<Doctor, DoctorDto>(
            paged,
            _mapper,
            filter.NormalizedPage,
            filter.NormalizedPageSize));
  }
}
