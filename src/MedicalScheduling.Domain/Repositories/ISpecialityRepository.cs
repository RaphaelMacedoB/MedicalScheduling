namespace MedicalScheduling.Domain.Repositories;

public interface ISpecialityRepository
{
  Task<Speciality?> GetByIdAsync(Guid id, CancellationToken ct = default);
  Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
  Task<IEnumerable<Speciality>> GetAllAsync(CancellationToken ct = default);
  Task AddAsync(Speciality Speciality, CancellationToken ct = default);
  void Update(Speciality Speciality);
}