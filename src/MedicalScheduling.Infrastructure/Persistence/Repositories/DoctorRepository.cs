using MedicalScheduling.Domain;
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
        await _context.Doctors
            .AnyAsync(d => d.Crm == crm, ct);

    public async Task<IEnumerable<Doctor>> GetBySpecialityAsync(Guid SpecialityId, CancellationToken ct = default) =>
        await _context.Doctors
            .Where(d => d.SpecialityId == SpecialityId && d.IsActive)
            .ToListAsync(ct);

    public async Task AddAsync(Doctor doctor, CancellationToken ct = default) =>
        await _context.Doctors.AddAsync(doctor, ct);

    public void Update(Doctor doctor) =>
        _context.Doctors.Update(doctor);
}