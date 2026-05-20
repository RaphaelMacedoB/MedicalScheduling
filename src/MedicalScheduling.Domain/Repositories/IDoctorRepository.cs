namespace MedicalScheduling.Domain.Repositories;

public interface IDoctorRepository
{
  Task<Doctor?> GetByIdAsync(Guid id, CancellationToken ct = default);
  Task<bool> ExistsByCrmAsync(string crm, CancellationToken ct = default);
  Task<IEnumerable<Doctor>> GetBySpecialityAsync(Guid SpecialityId, CancellationToken ct = default);
  Task AddAsync(Doctor doctor, CancellationToken ct = default);
  void Update(Doctor doctor);
}