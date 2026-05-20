using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Common;
using MedicalScheduling.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MedicalScheduling.Infrastructure.Persistence.Repositories;

public sealed class DoctorRepository : IDoctorRepository
{
  private readonly AppDbContext _context;

  public DoctorRepository(AppDbContext context) => _context = context;

  public async Task<Doctor?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
      await _context.Doctors
          .Include(d => d.Speciality)
          .FirstOrDefaultAsync(d => d.Id == id, ct);

  public async Task<bool> ExistsByCrmAsync(string crm, CancellationToken ct = default) =>
      await _context.Doctors.AnyAsync(d => d.Crm == crm, ct);

  public async Task<PagedResult<Doctor>> GetPagedAsync(
      int skip,
      int take,
      string? search,
      Guid? specialityId,
      bool? isActive,
      string? sortBy,
      bool sortDescending,
      CancellationToken ct = default)
  {
    var query = _context.Doctors
        .Include(d => d.Speciality)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
      var term = search.Trim().ToLower();
      query = query.Where(d =>
          d.Name.ToLower().Contains(term)
          || d.Crm.ToLower().Contains(term)
          || d.Email.Value.ToLower().Contains(term)
          || d.Phone.Value.Contains(term)
          || d.Speciality.Name.ToLower().Contains(term));
    }

    if (specialityId.HasValue)
      query = query.Where(d => d.SpecialityId == specialityId.Value);

    if (isActive.HasValue)
      query = query.Where(d => d.IsActive == isActive.Value);

    query = ApplySorting(query, sortBy, sortDescending);

    return await query.ToPagedResultAsync(skip, take, ct);
  }

  public async Task AddAsync(Doctor doctor, CancellationToken ct = default) =>
      await _context.Doctors.AddAsync(doctor, ct);

  public void Update(Doctor doctor) => _context.Doctors.Update(doctor);

  private static IQueryable<Doctor> ApplySorting(
      IQueryable<Doctor> query,
      string? sortBy,
      bool sortDescending)
  {
    return (sortBy?.ToLowerInvariant()) switch
    {
      "crm" => sortDescending ? query.OrderByDescending(d => d.Crm) : query.OrderBy(d => d.Crm),
      "email" => sortDescending ? query.OrderByDescending(d => d.Email.Value) : query.OrderBy(d => d.Email.Value),
      "speciality" => sortDescending
          ? query.OrderByDescending(d => d.Speciality.Name)
          : query.OrderBy(d => d.Speciality.Name),
      "createdat" => sortDescending
          ? query.OrderByDescending(d => d.CreatedAt)
          : query.OrderBy(d => d.CreatedAt),
      "isactive" => sortDescending
          ? query.OrderByDescending(d => d.IsActive)
          : query.OrderBy(d => d.IsActive),
      _ => sortDescending ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name)
    };
  }
}
