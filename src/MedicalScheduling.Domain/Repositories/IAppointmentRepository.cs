using MedicalScheduling.Domain.Common;

namespace MedicalScheduling.Domain.Repositories;

public interface IAppointmentRepository
{
  Task<Appointment?> GetByIdAsync(Guid id, CancellationToken ct = default);
  Task<PagedResult<Appointment>> GetPagedAsync(
      int skip,
      int take,
      string? search,
      Guid? patientId,
      Guid? doctorId,
      Guid? specialityId,
      string? status,
      DateTime? startFrom,
      DateTime? startTo,
      string? sortBy,
      bool sortDescending,
      CancellationToken ct = default);
  Task<bool> HasConflictAsync(Guid doctorId, DateTime start, DateTime end, CancellationToken ct = default);
  Task AddAsync(Appointment appointment, CancellationToken ct = default);
  void Update(Appointment appointment);
}
