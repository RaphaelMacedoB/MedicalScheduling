using MedicalScheduling.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalScheduling.Infrastructure.Persistence.Mappings;

internal sealed class SpecialityMapping : IEntityTypeConfiguration<Speciality>
{
  public void Configure(EntityTypeBuilder<Speciality> builder)
  {
    builder.ToTable("specialities");

    builder.ConfigureEntity();

    builder.Property(s => s.Name)
        .HasMaxLength(100)
        .IsRequired();

    builder.Property(s => s.Description)
        .HasMaxLength(500);

    builder.HasIndex(s => s.Name)
        .IsUnique();

    builder.HasMany(s => s.Doctors)
        .WithOne(d => d.Speciality)
        .HasForeignKey(d => d.SpecialityId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}
