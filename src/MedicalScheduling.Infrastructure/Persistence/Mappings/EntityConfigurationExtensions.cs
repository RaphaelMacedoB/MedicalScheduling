using MedicalScheduling.Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalScheduling.Infrastructure.Persistence.Mappings;

internal static class EntityConfigurationExtensions
{
  public static void ConfigureEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
      where TEntity : Entity
  {
    builder.HasKey(e => e.Id);

    builder.Property(e => e.Id)
        .ValueGeneratedNever();

    builder.Property(e => e.CreatedAt)
        .IsRequired();

    builder.Property(e => e.UpdatedAt);
  }
}
