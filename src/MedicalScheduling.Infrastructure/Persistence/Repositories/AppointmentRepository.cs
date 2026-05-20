using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Enums;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicalScheduling.Infrastructure.Persistence.Repositories;

public sealed class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context) => _context = context;

    public async Task<Appointment?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.Speciality)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IEnumerable<Appointment>> GetByDoctorAsync(Guid doctorId, CancellationToken ct = default) =>
        await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.Speciality)
            .Where(a => a.DoctorId == doctorId)
            .OrderBy(a => a.TimeSlot.Start)
            .ToListAsync(ct);

    public async Task<IEnumerable<Appointment>> GetByPatientAsync(Guid patientId, CancellationToken ct = default) =>
        await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.Speciality)
            .Where(a => a.PatientId == patientId)
            .OrderBy(a => a.TimeSlot.Start)
            .ToListAsync(ct);

    public async Task<bool> HasConflictAsync(Guid doctorId, DateTime start, DateTime end, CancellationToken ct = default) =>
        await _context.Appointments
            .Where(a => a.DoctorId == doctorId
                && a.Status != EAppointmentStatus.Cancelled
                && a.TimeSlot.Start < end
                && a.TimeSlot.End > start)
            .AnyAsync(ct);

    public async Task AddAsync(Appointment appointment, CancellationToken ct = default) =>
        await _context.Appointments.AddAsync(appointment, ct);

    public void Update(Appointment appointment) =>
        _context.Appointments.Update(appointment);
}