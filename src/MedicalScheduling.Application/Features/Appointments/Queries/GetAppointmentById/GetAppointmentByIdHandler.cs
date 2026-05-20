using AutoMapper;
using MediatR;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Appointments.GetAppointmentById;

public sealed class GetAppointmentByIdHandler : IRequestHandler<GetAppointmentByIdQuery, Result<AppointmentDto>>
{
  private readonly IAppointmentRepository _repository;
  private readonly IMapper _mapper;

  public GetAppointmentByIdHandler(IAppointmentRepository repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<Result<AppointmentDto>> Handle(GetAppointmentByIdQuery request, CancellationToken ct)
  {
    var appointment = await _repository.GetByIdAsync(request.Id, ct);
    if (appointment is null)
      return Result.Failure<AppointmentDto>(DomainErrors.Appointment.NotFound);

    return Result.Success(_mapper.Map<AppointmentDto>(appointment));
  }
}