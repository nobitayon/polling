using Delta.Polling.Domain.Interfaces;
using Delta.Polling.Infrastructure.Database.Statics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delta.Polling.Infrastructure.Database.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static void ConfigureCreatableProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, ICreatable
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(x => x.CreatedBy)
            .HasColumnType(ColumnTypeFor.NVarchar(BaseMaxLengthFor.Username));
    }

    public static void ConfigureModifiableProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IModifiable
    {
        builder.ConfigureCreatableProperties();

        _ = builder.Property(x => x.ModifiedBy)
            .HasColumnType(ColumnTypeFor.NVarchar(BaseMaxLengthFor.Username));
    }

    public static void ConfigureFileProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IHasFile
    {
        builder.ConfigureModifiableProperties();

        _ = builder.Property(x => x.FileName).HasMaxLength(BaseMaxLengthFor.FileName);
        _ = builder.Property(x => x.FileContentType).HasMaxLength(BaseMaxLengthFor.FileContentType);
        _ = builder.Property(x => x.StoredFileId).HasMaxLength(BaseMaxLengthFor.StoredFileId);
    }
}
