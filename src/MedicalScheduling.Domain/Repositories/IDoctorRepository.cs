using MedicalScheduling.Domain.Common;

namespace MedicalScheduling.Domain.Repositories;

public interface IDoctorRepository
{
  Task<Doctor?> GetByIdAsync(Guid id, CancellationToken ct = default);
  Task<bool> ExistsByCrmAsync(string crm, CancellationToken ct = default);
  Task<PagedResult<Doctor>> GetPagedAsync(
      int skip,
      int take,
      string? search,
      Guid? specialityId,
      bool? isActive,
      string? sortBy,
      bool sortDescending,
      CancellationToken ct = default);
  Task AddAsync(Doctor doctor, CancellationToken ct = default);
  void Update(Doctor doctor);
}
