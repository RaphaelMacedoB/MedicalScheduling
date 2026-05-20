using AutoMapper;
using MediatR;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Appointments.GetAppointmentsByPatient;

public sealed class GetAppointmentsByPatientHandler
    : IRequestHandler<GetAppointmentsByPatientQuery, Result<IEnumerable<AppointmentDto>>>
{
  private readonly IAppointmentRepository _repository;
  private readonly IMapper _mapper;

  public GetAppointmentsByPatientHandler(IAppointmentRepository repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<Result<IEnumerable<AppointmentDto>>> Handle(GetAppointmentsByPatientQuery request, CancellationToken ct)
  {
    var appointments = await _repository.GetByPatientAsync(request.PatientId, ct);
    return Result.Success(_mapper.Map<IEnumerable<AppointmentDto>>(appointments));
  }
}