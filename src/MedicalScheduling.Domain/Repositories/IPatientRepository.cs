namespace MedicalScheduling.Domain.Repositories;

public interface IPatientRepository
{
  Task<Patient?> GetByIdAsync(Guid id, CancellationToken ct = default);
  Task<Patient?> GetByCpfAsync(string cpf, CancellationToken ct = default);
  Task<bool> ExistsByCpfAsync(string cpf, CancellationToken ct = default);
  Task<IEnumerable<Patient>> GetAllAsync(CancellationToken ct = default);
  Task AddAsync(Patient patient, CancellationToken ct = default);
  void Update(Patient patient);
}