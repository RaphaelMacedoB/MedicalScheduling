using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalScheduling.Infrastructure.Persistence.Mappings;

internal sealed class AppointmentMapping : IEntityTypeConfiguration<Appointment>
{
  public void Configure(EntityTypeBuilder<Appointment> builder)
  {
    builder.ToTable("appointments");

    builder.ConfigureEntity();

    builder.Property(a => a.PatientId)
        .IsRequired();

    builder.Property(a => a.DoctorId)
        .IsRequired();

    builder.OwnsOne(a => a.TimeSlot, timeSlot =>
    {
      timeSlot.Property(t => t.Start)
          .HasColumnName("start_at")
          .IsRequired();

      timeSlot.Property(t => t.End)
          .HasColumnName("end_at")
          .IsRequired();
    });

    builder.Property(a => a.Status)
        .HasConversion<string>()
        .HasMaxLength(20)
        .IsRequired();

    builder.Property(a => a.Notes)
        .HasMaxLength(1000);

    builder.HasIndex(a => a.DoctorId);
    builder.HasIndex(a => a.PatientId);
    builder.HasIndex(a => new { a.DoctorId, a.Status });
  }
}
