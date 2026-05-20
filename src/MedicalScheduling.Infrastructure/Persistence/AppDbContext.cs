using MedicalScheduling.Domain;
using Microsoft.EntityFrameworkCore;

namespace MedicalScheduling.Infrastructure.Persistence.Repositories;

public sealed class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

  public DbSet<Patient> Patients => Set<Patient>();
  public DbSet<Doctor> Doctors => Set<Doctor>();
  public DbSet<Appointment> Appointments => Set<Appointment>();
  public DbSet<Speciality> Specialities => Set<Speciality>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    base.OnModelCreating(modelBuilder);
  }
}