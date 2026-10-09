using MediCare.Domain.Permissions;
using MediCare.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Persistence.Repositories;

public sealed class PermissionRepository(AppDbContext dbContext) : IPermissionRepository
{
    public Task<List<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.Permissions.ToListAsync(cancellationToken);
    }

    public async Task<List<Permission>> GetByIdsAsync(List<PermissionId> ids, CancellationToken cancellationToken)
    {
        return await dbContext.Permissions
        .Where(p => ids.Contains(p.Id))
        .ToListAsync(cancellationToken);
    }

    public Task<Permission?> GetByIdAsync(PermissionId id, CancellationToken cancellationToken = default)
    {
        return dbContext.Permissions.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
