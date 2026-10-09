using MediCare.Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Persistence.Configurations;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnName("PermissionID")
            .HasConversion(id => id.Value, value => new PermissionId(value))
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .HasColumnName("PermissionName")
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(p => p.Name).IsUnique();

        builder.HasData(PermissionCatalog.DisplayNames.Keys.Select(code => new
        {
            Id = new PermissionId(PermissionCatalog.IdOf(code)),
            Name = code
        }));

        builder.Ignore(p => p.DomainEvents);
    }
}
