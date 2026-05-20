using MedicalScheduling.Domain;
using MedicalScheduling.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalScheduling.Infrastructure.Persistence.Mappings;

internal sealed class PatientMapping : IEntityTypeConfiguration<Patient>
{
  public void Configure(EntityTypeBuilder<Patient> builder)
  {
    builder.ToTable("patients");

    builder.ConfigureEntity();

    builder.Property(p => p.Name)
        .HasMaxLength(200)
        .IsRequired();

    builder.Property(p => p.Email)
        .HasConversion(ValueObjectConverters.EmailConverter)
        .HasMaxLength(256)
        .HasColumnName("email")
        .IsRequired();

    builder.Property(p => p.Cpf)
        .HasConversion(ValueObjectConverters.CpfConverter)
        .HasMaxLength(11)
        .HasColumnName("cpf")
        .IsRequired();

    builder.HasIndex(p => p.Cpf)
        .IsUnique();

    builder.Property(p => p.Phone)
        .HasConversion(ValueObjectConverters.PhoneConverter)
        .HasMaxLength(11)
        .HasColumnName("phone")
        .IsRequired();

    builder.Property(p => p.BirthDate)
        .IsRequired();

    builder.Property(p => p.IsActive)
        .IsRequired();

    builder.HasMany(p => p.Appointments)
        .WithOne(a => a.Patient)
        .HasForeignKey(a => a.PatientId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}
