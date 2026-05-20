using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Common;
using MedicalScheduling.Domain.Enums;
using MedicalScheduling.Domain.Repositories;
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

  public async Task<PagedResult<Appointment>> GetPagedAsync(
      int skip,
      int take,
      string? search,
      Guid? patientId,
      Guid? doctorId,
      Guid? specialityId,
      string? status,
      DateTime? startFrom,
      DateTime? startTo,
      string? sortBy,
      bool sortDescending,
      CancellationToken ct = default)
  {
    var query = _context.Appointments
        .Include(a => a.Patient)
        .Include(a => a.Doctor)
            .ThenInclude(d => d.Speciality)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
      var term = search.Trim().ToLower();
      query = query.Where(a =>
          a.Patient.Name.ToLower().Contains(term)
          || a.Doctor.Name.ToLower().Contains(term)
          || a.Doctor.Crm.ToLower().Contains(term)
          || a.Doctor.Speciality.Name.ToLower().Contains(term)
          || (a.Notes != null && a.Notes.ToLower().Contains(term)));
    }

    if (patientId.HasValue)
      query = query.Where(a => a.PatientId == patientId.Value);

    if (doctorId.HasValue)
      query = query.Where(a => a.DoctorId == doctorId.Value);

    if (specialityId.HasValue)
      query = query.Where(a => a.Doctor.SpecialityId == specialityId.Value);

    if (!string.IsNullOrWhiteSpace(status)
        && Enum.TryParse<EAppointmentStatus>(status, true, out var parsedStatus))
    {
      query = query.Where(a => a.Status == parsedStatus);
    }

    if (startFrom.HasValue)
      query = query.Where(a => a.TimeSlot.Start >= startFrom.Value);

    if (startTo.HasValue)
      query = query.Where(a => a.TimeSlot.Start <= startTo.Value);

    query = ApplySorting(query, sortBy, sortDescending);

    return await query.ToPagedResultAsync(skip, take, ct);
  }

  public async Task<bool> HasConflictAsync(Guid doctorId, DateTime start, DateTime end, CancellationToken ct = default) =>
      await _context.Appointments
          .Where(a => a.DoctorId == doctorId
              && a.Status != EAppointmentStatus.Cancelled
              && a.TimeSlot.Start < end
              && a.TimeSlot.End > start)
          .AnyAsync(ct);

  public async Task AddAsync(Appointment appointment, CancellationToken ct = default) =>
      await _context.Appointments.AddAsync(appointment, ct);

  public void Update(Appointment appointment) => _context.Appointments.Update(appointment);

  private static IQueryable<Appointment> ApplySorting(
      IQueryable<Appointment> query,
      string? sortBy,
      bool sortDescending)
  {
    return (sortBy?.ToLowerInvariant()) switch
    {
      "status" => sortDescending
          ? query.OrderByDescending(a => a.Status)
          : query.OrderBy(a => a.Status),
      "patient" => sortDescending
          ? query.OrderByDescending(a => a.Patient.Name)
          : query.OrderBy(a => a.Patient.Name),
      "doctor" => sortDescending
          ? query.OrderByDescending(a => a.Doctor.Name)
          : query.OrderBy(a => a.Doctor.Name),
      "end" or "endat" => sortDescending
          ? query.OrderByDescending(a => a.TimeSlot.End)
          : query.OrderBy(a => a.TimeSlot.End),
      "createdat" => sortDescending
          ? query.OrderByDescending(a => a.CreatedAt)
          : query.OrderBy(a => a.CreatedAt),
      _ => sortDescending
          ? query.OrderByDescending(a => a.TimeSlot.Start)
          : query.OrderBy(a => a.TimeSlot.Start)
    };
  }
}
