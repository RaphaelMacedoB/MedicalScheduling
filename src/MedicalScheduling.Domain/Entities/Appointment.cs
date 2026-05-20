using MedicalScheduling.Domain.Enums;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.ValueObjects;

namespace MedicalScheduling.Domain;

public sealed class Appointment : Entity
{
  public Guid PatientId { get; private set; }
  public Patient Patient { get; private set; } = null!;

  public Guid DoctorId { get; private set; }
  public Doctor Doctor { get; private set; } = null!;

  public TimeSlot TimeSlot { get; private set; } = null!;
  public EAppointmentStatus Status { get; private set; }
  public string? Notes { get; private set; }

  private Appointment() { } // EF Core

  private Appointment(Guid patientId, Guid doctorId, TimeSlot timeSlot)
  {
    PatientId = patientId;
    DoctorId = doctorId;
    TimeSlot = timeSlot;
    Status = EAppointmentStatus.Scheduled;
  }

  public static Result<Appointment> Schedule(
      Guid patientId,
      Guid doctorId,
      DateTime start,
      DateTime end)
  {
    var slotResult = TimeSlot.Create(start, end);
    if (slotResult.IsFailure) return Result.Failure<Appointment>(slotResult.Error);

    return Result.Success(new Appointment(patientId, doctorId, slotResult.Value));
  }

  public Result Confirm()
  {
    if (Status == EAppointmentStatus.Cancelled)
      return Result.Failure(DomainErrors.Appointment.AlreadyCancelled);

    Status = EAppointmentStatus.Confirmed;
    UpdatedAt = DateTime.UtcNow;
    return Result.Success();
  }

  public Result Complete(string? notes = null)
  {
    if (Status == EAppointmentStatus.Cancelled)
      return Result.Failure(DomainErrors.Appointment.AlreadyCancelled);

    Status = EAppointmentStatus.Completed;
    Notes = notes;
    UpdatedAt = DateTime.UtcNow;
    return Result.Success();
  }

  public Result Cancel()
  {
    if (Status == EAppointmentStatus.Cancelled)
      return Result.Failure(DomainErrors.Appointment.AlreadyCancelled);

    if (Status == EAppointmentStatus.Completed)
      return Result.Failure(DomainErrors.Appointment.AlreadyCompleted);

    Status = EAppointmentStatus.Cancelled;
    UpdatedAt = DateTime.UtcNow;
    return Result.Success();
  }
}