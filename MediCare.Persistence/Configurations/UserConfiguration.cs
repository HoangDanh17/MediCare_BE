using MediCare.Domain.Roles;
using MediCare.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        // Khóa chính
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("UserID")
            .HasConversion(id => id.Value, value => new UserId(value))
            .ValueGeneratedNever();

        // Khóa ngoại tới Roles
        builder.Property(u => u.RoleId)
            .HasColumnName("RoleID")
            .HasConversion(id => id.Value, value => new RoleId(value));

        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Các cột thông thường
        builder.Property(u => u.FullName).IsRequired().HasMaxLength(200);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(500);
        builder.Property(u => u.PhoneNumber).HasColumnName("Phone").IsRequired().HasMaxLength(20);
        builder.Property(u => u.Address).IsRequired().HasMaxLength(500);
        builder.Property(u => u.Gender).HasConversion<string>().HasMaxLength(20);
        builder.Property(u => u.Dob).IsRequired();
        builder.Property(u => u.IsActive).IsRequired();
        builder.Property(u => u.CreatedAt).IsRequired();

        // Chốt chặn email trùng ở mức DB
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.RoleId);

        // Quyền ngoại lệ của user
        builder.HasMany(u => u.Permissions)
            .WithOne()
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(u => u.Permissions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Domain event chỉ sống trong bộ nhớ, không lưu xuống DB
        builder.Ignore(u => u.DomainEvents);
    }
}
