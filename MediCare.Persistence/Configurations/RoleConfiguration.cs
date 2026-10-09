using MediCare.Domain.Permissions;
using MediCare.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
           .HasColumnName("RoleID")
           .HasConversion(id => id.Value, value => new RoleId(value))
           .ValueGeneratedNever();

        builder.Property(r => r.Name)
            .HasColumnName("RoleName")
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(r => r.Name).IsUnique();

        builder.HasMany(r => r.Permissions)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "Role_Permission",
                right => right
                    .HasOne<Permission>()
                    .WithMany()
                    .HasForeignKey("PermissionID")
                    .OnDelete(DeleteBehavior.Restrict),
                left => left
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleID")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("Role_Permission");
                    join.HasKey("RoleID", "PermissionID");
                    join.HasIndex("PermissionID");
                });

        builder.Navigation(r => r.Permissions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(r => r.DomainEvents);
    }
}
