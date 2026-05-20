using System.Text.RegularExpressions;
using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.Domain.ValueObjects;
using MedicalScheduling.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicalScheduling.Infrastructure.Persistence.Repositories;

public sealed class PatientRepository : IPatientRepository
{
  private readonly AppDbContext _context;

  public PatientRepository(AppDbContext context) => _context = context;

  public async Task<Patient?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
      await _context.Patients
          .FirstOrDefaultAsync(p => p.Id == id, ct);

  public async Task<Patient?> GetByCpfAsync(string cpf, CancellationToken ct = default)
  {
    var cpfValue = Cpf.FromPersistence(NormalizeCpf(cpf));
    return await _context.Patients
        .FirstOrDefaultAsync(p => p.Cpf == cpfValue, ct);
  }

  public async Task<bool> ExistsByCpfAsync(string cpf, CancellationToken ct = default)
  {
    var cpfValue = Cpf.FromPersistence(NormalizeCpf(cpf));
    return await _context.Patients
        .AnyAsync(p => p.Cpf == cpfValue, ct);
  }

  public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken ct = default) =>
      await _context.Patients
          .Where(p => p.IsActive)
          .ToListAsync(ct);

  public async Task AddAsync(Patient patient, CancellationToken ct = default) =>
      await _context.Patients.AddAsync(patient, ct);

  public void Update(Patient patient) =>
      _context.Patients.Update(patient);

  private static string NormalizeCpf(string cpf) => Regex.Replace(cpf, @"\D", "");
}
