using System.Text.RegularExpressions;
using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Common;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MedicalScheduling.Infrastructure.Persistence.Repositories;

public sealed class PatientRepository : IPatientRepository
{
  private readonly AppDbContext _context;

  public PatientRepository(AppDbContext context) => _context = context;

  public async Task<Patient?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
      await _context.Patients.FirstOrDefaultAsync(p => p.Id == id, ct);

  public async Task<Patient?> GetByCpfAsync(string cpf, CancellationToken ct = default)
  {
    var cpfValue = Cpf.FromPersistence(NormalizeCpf(cpf));
    return await _context.Patients.FirstOrDefaultAsync(p => p.Cpf == cpfValue, ct);
  }

  public async Task<bool> ExistsByCpfAsync(string cpf, CancellationToken ct = default)
  {
    var cpfValue = Cpf.FromPersistence(NormalizeCpf(cpf));
    return await _context.Patients.AnyAsync(p => p.Cpf == cpfValue, ct);
  }

  public async Task<PagedResult<Patient>> GetPagedAsync(
      int skip,
      int take,
      string? search,
      bool? isActive,
      DateOnly? birthDateFrom,
      DateOnly? birthDateTo,
      string? sortBy,
      bool sortDescending,
      CancellationToken ct = default)
  {
    var query = _context.Patients.AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
      var term = search.Trim().ToLower();
      query = query.Where(p =>
          p.Name.ToLower().Contains(term)
          || p.Email.Value.ToLower().Contains(term)
          || p.Cpf.Value.Contains(term)
          || p.Phone.Value.Contains(term));
    }

    if (isActive.HasValue)
      query = query.Where(p => p.IsActive == isActive.Value);

    if (birthDateFrom.HasValue)
      query = query.Where(p => p.BirthDate >= birthDateFrom.Value);

    if (birthDateTo.HasValue)
      query = query.Where(p => p.BirthDate <= birthDateTo.Value);

    query = ApplySorting(query, sortBy, sortDescending);

    return await query.ToPagedResultAsync(skip, take, ct);
  }

  public async Task AddAsync(Patient patient, CancellationToken ct = default) =>
      await _context.Patients.AddAsync(patient, ct);

  public void Update(Patient patient) => _context.Patients.Update(patient);

  private static IQueryable<Patient> ApplySorting(
      IQueryable<Patient> query,
      string? sortBy,
      bool sortDescending)
  {
    return (sortBy?.ToLowerInvariant()) switch
    {
      "name" => sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
      "email" => sortDescending ? query.OrderByDescending(p => p.Email.Value) : query.OrderBy(p => p.Email.Value),
      "cpf" => sortDescending ? query.OrderByDescending(p => p.Cpf.Value) : query.OrderBy(p => p.Cpf.Value),
      "birthdate" => sortDescending ? query.OrderByDescending(p => p.BirthDate) : query.OrderBy(p => p.BirthDate),
      "createdat" => sortDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
      "isactive" => sortDescending ? query.OrderByDescending(p => p.IsActive) : query.OrderBy(p => p.IsActive),
      _ => sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name)
    };
  }

  private static string NormalizeCpf(string cpf) => Regex.Replace(cpf, @"\D", "");
}
