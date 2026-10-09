using MediCare.Domain.Repositories;
using MediCare.Domain.Roles;
using MediCare.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Persistence.Repositories;

public sealed class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        dbContext.Users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        dbContext.Users.Remove(user);
        return Task.CompletedTask;
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public Task<bool> ExistsRoleInUseAsync(RoleId roleId, CancellationToken cancellationToken = default)
        => dbContext.Users.AnyAsync(u => u.RoleId == roleId, cancellationToken);

    public Task<bool> ExistsRoleInUseAsync(RoleId roleId, UserId excludeUserId, CancellationToken cancellationToken = default)
        => dbContext.Users.AnyAsync(u => u.RoleId == roleId && u.Id != excludeUserId, cancellationToken);

    public async Task<List<User>> GetPageAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default)
    {
        return await ApplySearch(dbContext.Users.AsNoTracking(), search)
            .OrderBy(u => u.FullName)
            .ThenBy(u => u.Email)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(string? search, CancellationToken cancellationToken = default)
        => ApplySearch(dbContext.Users.AsNoTracking(), search).CountAsync(cancellationToken);

    private static IQueryable<User> ApplySearch(IQueryable<User> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return query;

        var term = search.Trim().ToLower();
        return query.Where(u => u.FullName.ToLower().Contains(term) || u.Email.ToLower().Contains(term));
    }

    public async Task<HashSet<string>> GetEffectivePermissionCodesAsync(
    UserId userId, CancellationToken cancellationToken = default)
    {
        var isActive = await dbContext.Users.AsNoTracking()
            .AnyAsync(u => u.Id == userId && u.IsActive, cancellationToken);

        if (!isActive)
            return [];

        var user = await dbContext.Users.AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new { u.RoleId })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return [];

        var all = await dbContext.Permissions.AsNoTracking()
            .Select(p => new { p.Id, p.Name })
            .ToListAsync(cancellationToken);

        var rolePermissionIds = (await dbContext.Roles.AsNoTracking()
                .Where(r => r.Id == user.RoleId)
                .SelectMany(r => r.Permissions)
                .Select(p => p.Id)
                .ToListAsync(cancellationToken))
            .ToHashSet();

        var overrides = (await dbContext.Set<UserPermission>().AsNoTracking()
                .Where(up => up.UserId == userId)
                .Select(up => new { up.PermissionId, up.IsGranted })
                .ToListAsync(cancellationToken))
            .ToDictionary(x => x.PermissionId, x => x.IsGranted);

        return all
            .Where(p => overrides.TryGetValue(p.Id, out var isGranted)
                ? isGranted
                : rolePermissionIds.Contains(p.Id))
            .Select(p => p.Name)
            .ToHashSet();
    }
}
