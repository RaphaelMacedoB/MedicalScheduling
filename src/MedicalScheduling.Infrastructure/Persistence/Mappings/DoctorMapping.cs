using MedicalScheduling.Domain;
using MedicalScheduling.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalScheduling.Infrastructure.Persistence.Mappings;

internal sealed class DoctorMapping : IEntityTypeConfiguration<Doctor>
{
  public void Configure(EntityTypeBuilder<Doctor> builder)
  {
    builder.ToTable("doctors");

    builder.ConfigureEntity();

    builder.Property(d => d.Name)
        .HasMaxLength(200)
        .IsRequired();

    builder.Property(d => d.Crm)
        .HasMaxLength(20)
        .IsRequired();

    builder.HasIndex(d => d.Crm)
        .IsUnique();

    builder.Property(d => d.Email)
        .HasConversion(ValueObjectConverters.EmailConverter)
        .HasMaxLength(256)
        .HasColumnName("email")
        .IsRequired();

    builder.Property(d => d.Phone)
        .HasConversion(ValueObjectConverters.PhoneConverter)
        .HasMaxLength(11)
        .HasColumnName("phone")
        .IsRequired();

    builder.Property(d => d.SpecialityId)
        .IsRequired();

    builder.Property(d => d.IsActive)
        .IsRequired();

    builder.HasMany(d => d.Appointments)
        .WithOne(a => a.Doctor)
        .HasForeignKey(a => a.DoctorId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}
