using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicalScheduling.Infrastructure.Persistence.Repositories;

public sealed class SpecialityRepository : ISpecialityRepository
{
  private readonly AppDbContext _context;

  public SpecialityRepository(AppDbContext context) => _context = context;

  public async Task<Speciality?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
      await _context.Specialities
          .FirstOrDefaultAsync(s => s.Id == id, ct);

  public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default) =>
      await _context.Specialities
          .AnyAsync(s => s.Name.ToLower() == name.ToLower(), ct);

  public async Task<IEnumerable<Speciality>> GetAllAsync(CancellationToken ct = default) =>
      await _context.Specialities
          .OrderBy(s => s.Name)
          .ToListAsync(ct);

  public async Task AddAsync(Speciality Speciality, CancellationToken ct = default) =>
      await _context.Specialities.AddAsync(Speciality, ct);

  public void Update(Speciality Speciality) =>
      _context.Specialities.Update(Speciality);
}