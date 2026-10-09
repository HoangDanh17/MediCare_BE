using MediCare.Application.Common.Interfaces;
using MediCare.Contract.User;
using MediCare.Domain.Permissions;
using MediCare.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Persistence.Common;

public sealed class UserPermissionReader(AppDbContext db) : IUserPermissionReader
{
    public async Task<List<UserPermissionStateResponse>?> GetStatesAsync(
        UserId userId, CancellationToken cancellationToken = default)
    {
        var user = await db.Users.AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new { u.RoleId })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return null;

        var all = await db.Permissions.AsNoTracking()
            .Select(p => new { p.Id, p.Name })
            .ToListAsync(cancellationToken);

        var rolePermissionIds = (await db.Roles.AsNoTracking()
                .Where(r => r.Id == user.RoleId)
                .SelectMany(r => r.Permissions)
                .Select(p => p.Id)
                .ToListAsync(cancellationToken))
            .ToHashSet();

        var overrides = (await db.Set<UserPermission>().AsNoTracking()
                .Where(up => up.UserId == userId)
                .Select(up => new { up.PermissionId, up.IsGranted })
                .ToListAsync(cancellationToken))
            .ToDictionary(x => x.PermissionId, x => x.IsGranted);

        return all
            .OrderBy(p => p.Name)
            .Select(p =>
            {
                var fromRole = rolePermissionIds.Contains(p.Id);
                var hasOverride = overrides.TryGetValue(p.Id, out var isGranted);

                var setting = !hasOverride ? "Inherit" : isGranted ? "Allow" : "Deny";
                var hasPermission = hasOverride ? isGranted : fromRole;

                return new UserPermissionStateResponse(
                    p.Id.Value,
                    p.Name,
                    PermissionCatalog.DisplayNames.GetValueOrDefault(p.Name, p.Name),
                    fromRole,
                    setting,
                    hasPermission);
            })
            .ToList();
    }

    public async Task<HashSet<string>> GetEffectivePermissionCodesAsync(
        UserId userId, CancellationToken cancellationToken = default)
    {
        var states = await GetStatesAsync(userId, cancellationToken);

        if (states is null)
            return [];

        return states.Where(s => s.HasPermission).Select(s => s.Name).ToHashSet();
    }
}
