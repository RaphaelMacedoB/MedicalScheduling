namespace MedicalScheduling.Application.Features.Appointments.ScheduleAppointment;

using AutoMapper;
using MediatR;
using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.Infrastructure;

public sealed class ScheduleAppointmentHandler : IRequestHandler<ScheduleAppointmentCommand, Result<AppointmentDto>>
{
  private readonly IAppointmentRepository _appointmentRepository;
  private readonly IPatientRepository _patientRepository;
  private readonly IDoctorRepository _doctorRepository;
  private readonly IUnitOfWork _uow;
  private readonly IMapper _mapper;

  public ScheduleAppointmentHandler(
      IAppointmentRepository appointmentRepository,
      IPatientRepository patientRepository,
      IDoctorRepository doctorRepository,
      IUnitOfWork uow,
      IMapper mapper)
  {
    _appointmentRepository = appointmentRepository;
    _patientRepository = patientRepository;
    _doctorRepository = doctorRepository;
    _uow = uow;
    _mapper = mapper;
  }

  public async Task<Result<AppointmentDto>> Handle(ScheduleAppointmentCommand request, CancellationToken ct)
  {
    var patient = await _patientRepository.GetByIdAsync(request.PatientId, ct);
    if (patient is null)
      return Result.Failure<AppointmentDto>(DomainErrors.Patient.NotFound);

    var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId, ct);
    if (doctor is null)
      return Result.Failure<AppointmentDto>(DomainErrors.Doctor.NotFound);

    var hasConflict = await _appointmentRepository.HasConflictAsync(
        request.DoctorId, request.Start, request.End, ct);

    if (hasConflict)
      return Result.Failure<AppointmentDto>(DomainErrors.Appointment.SlotUnavailable);

    var result = Appointment.Schedule(
        request.PatientId,
        request.DoctorId,
        request.Start,
        request.End);

    if (result.IsFailure)
      return Result.Failure<AppointmentDto>(result.Error);

    await _appointmentRepository.AddAsync(result.Value, ct);
    await _uow.SaveChangesAsync(ct);

    return Result.Success(_mapper.Map<AppointmentDto>(result.Value));
  }
}