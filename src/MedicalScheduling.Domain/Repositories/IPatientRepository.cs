using MedicalScheduling.Domain.Common;

namespace MedicalScheduling.Domain.Repositories;

public interface IPatientRepository
{
  Task<Patient?> GetByIdAsync(Guid id, CancellationToken ct = default);
  Task<Patient?> GetByCpfAsync(string cpf, CancellationToken ct = default);
  Task<bool> ExistsByCpfAsync(string cpf, CancellationToken ct = default);
  Task<PagedResult<Patient>> GetPagedAsync(
      int skip,
      int take,
      string? search,
      bool? isActive,
      DateOnly? birthDateFrom,
      DateOnly? birthDateTo,
      string? sortBy,
      bool sortDescending,
      CancellationToken ct = default);
  Task AddAsync(Patient patient, CancellationToken ct = default);
  void Update(Patient patient);
}
