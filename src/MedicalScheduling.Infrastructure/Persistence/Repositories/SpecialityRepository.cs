using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Common;
using MedicalScheduling.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MedicalScheduling.Infrastructure.Persistence.Repositories;

public sealed class SpecialityRepository : ISpecialityRepository
{
  private readonly AppDbContext _context;

  public SpecialityRepository(AppDbContext context) => _context = context;

  public async Task<Speciality?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
      await _context.Specialities.FirstOrDefaultAsync(s => s.Id == id, ct);

  public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default) =>
      await _context.Specialities.AnyAsync(s => s.Name.ToLower() == name.ToLower(), ct);

  public async Task<PagedResult<Speciality>> GetPagedAsync(
      int skip,
      int take,
      string? search,
      string? sortBy,
      bool sortDescending,
      CancellationToken ct = default)
  {
    var query = _context.Specialities.AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
      var term = search.Trim().ToLower();
      query = query.Where(s =>
          s.Name.ToLower().Contains(term)
          || (s.Description != null && s.Description.ToLower().Contains(term)));
    }

    query = ApplySorting(query, sortBy, sortDescending);

    return await query.ToPagedResultAsync(skip, take, ct);
  }

  public async Task AddAsync(Speciality speciality, CancellationToken ct = default) =>
      await _context.Specialities.AddAsync(speciality, ct);

  public void Update(Speciality speciality) => _context.Specialities.Update(speciality);

  private static IQueryable<Speciality> ApplySorting(
      IQueryable<Speciality> query,
      string? sortBy,
      bool sortDescending)
  {
    return (sortBy?.ToLowerInvariant()) switch
    {
      "description" => sortDescending
          ? query.OrderByDescending(s => s.Description)
          : query.OrderBy(s => s.Description),
      "createdat" => sortDescending
          ? query.OrderByDescending(s => s.CreatedAt)
          : query.OrderBy(s => s.CreatedAt),
      _ => sortDescending ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name)
    };
  }
}
