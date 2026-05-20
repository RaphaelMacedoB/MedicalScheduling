using MedicalScheduling.Domain.Common;

namespace MedicalScheduling.Domain.Repositories;

public interface ISpecialityRepository
{
  Task<Speciality?> GetByIdAsync(Guid id, CancellationToken ct = default);
  Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
  Task<PagedResult<Speciality>> GetPagedAsync(
      int skip,
      int take,
      string? search,
      string? sortBy,
      bool sortDescending,
      CancellationToken ct = default);
  Task AddAsync(Speciality speciality, CancellationToken ct = default);
  void Update(Speciality speciality);
}
