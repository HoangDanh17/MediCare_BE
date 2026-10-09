using MediCare.Domain.Majors;
using MediCare.Domain.Permissions;
using MediCare.Domain.Roles;
using MediCare.Domain.Rooms;
using MediCare.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Major> Majors => Set<Major>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Permission> Permissions => Set<Permission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
