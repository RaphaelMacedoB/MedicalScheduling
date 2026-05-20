using AutoMapper;
using MediatR;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Appointments.GetAppointmentsByDoctor;

public sealed class GetAppointmentsByDoctorHandler
    : IRequestHandler<GetAppointmentsByDoctorQuery, Result<IEnumerable<AppointmentDto>>>
{
  private readonly IAppointmentRepository _repository;
  private readonly IMapper _mapper;

  public GetAppointmentsByDoctorHandler(IAppointmentRepository repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<Result<IEnumerable<AppointmentDto>>> Handle(GetAppointmentsByDoctorQuery request, CancellationToken ct)
  {
    var appointments = await _repository.GetByDoctorAsync(request.DoctorId, ct);
    return Result.Success(_mapper.Map<IEnumerable<AppointmentDto>>(appointments));
  }
}