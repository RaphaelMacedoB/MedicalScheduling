using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.Infrastructure.Persistence;

namespace MedicalScheduling.Infrastructure;

public sealed class UnitOfWork : IUnitOfWork
{
  private readonly AppDbContext _context;

  public UnitOfWork(AppDbContext context) => _context = context;

  public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
      _context.SaveChangesAsync(ct);
}