namespace MedicalScheduling.Domain.Repositories;

public interface IAppointmentRepository
{
  Task<Appointment?> GetByIdAsync(Guid id, CancellationToken ct = default);
  Task<IEnumerable<Appointment>> GetByDoctorAsync(Guid doctorId, CancellationToken ct = default);
  Task<IEnumerable<Appointment>> GetByPatientAsync(Guid patientId, CancellationToken ct = default);
  Task<bool> HasConflictAsync(Guid doctorId, DateTime start, DateTime end, CancellationToken ct = default);
  Task AddAsync(Appointment appointment, CancellationToken ct = default);
  void Update(Appointment appointment);
}