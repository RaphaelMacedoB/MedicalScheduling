using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Common.Pagination;
using MedicalScheduling.Application.Features.Appointments.DTOs;
using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Appointments.Queries.GetAppointments;

public sealed class GetAppointmentsHandler
    : IRequestHandler<GetAppointmentsQuery, Result<PagedResponse<AppointmentDto>>>
{
  private readonly IAppointmentRepository _repository;
  private readonly IMapper _mapper;

  public GetAppointmentsHandler(IAppointmentRepository repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<Result<PagedResponse<AppointmentDto>>> Handle(GetAppointmentsQuery request, CancellationToken ct)
  {
    var filter = request.Filter;
    var paged = await _repository.GetPagedAsync(
        filter.Skip,
        filter.Take,
        filter.Search,
        filter.PatientId,
        filter.DoctorId,
        filter.SpecialityId,
        filter.Status,
        filter.StartFrom,
        filter.StartTo,
        filter.SortBy,
        filter.SortDescending,
        ct);

    return Result.Success(
        PaginationMapper.ToPagedResponse<Appointment, AppointmentDto>(
            paged,
            _mapper,
            filter.NormalizedPage,
            filter.NormalizedPageSize));
  }
}
