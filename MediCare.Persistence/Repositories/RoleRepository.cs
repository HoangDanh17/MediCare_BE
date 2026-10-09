using MediCare.Domain.Repositories;
using MediCare.Domain.Roles;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Persistence.Repositories;

public sealed class RoleRepository(AppDbContext dbContext) : IRoleRepository
{
    public Task AddAsync(Role role, CancellationToken cancellationToken = default)
    {
        dbContext.Roles.Add(role);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Role role, CancellationToken cancellationToken = default)
    {
        dbContext.Roles.Remove(role);
        return Task.CompletedTask;
    }

    public Task<List<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.Roles
            .Include(r => r.Permissions)
            .ToListAsync(cancellationToken);
    }

    public Task<Role?> GetByIdAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        return dbContext.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
    public Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return dbContext.Roles
         .Include(r => r.Permissions)
         .FirstOrDefaultAsync(r => r.Name == name.Trim(), cancellationToken);
    }

    public Task<bool> ExistsAsync(RoleId id, CancellationToken cancellationToken = default)
        => dbContext.Roles.AnyAsync(r => r.Id == id, cancellationToken);
}
