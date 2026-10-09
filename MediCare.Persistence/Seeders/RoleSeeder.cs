using MediCare.Domain.Common;
using MediCare.Domain.Permissions;
using MediCare.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MediCare.Persistence.Seeders;

public sealed class RoleSeeder(AppDbContext dbContext, ILogger<RoleSeeder> logger)
{
    private static readonly IReadOnlyDictionary<string, string[]> DesiredPermissions =
        new Dictionary<string, string[]>
        {
            [RoleNames.Admin] = [.. PermissionCatalog.DisplayNames.Keys],
            [RoleNames.User] =
            [
                PermissionCatalog.UserRead
            ]
        };

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var permissions = await dbContext.Permissions.ToListAsync(cancellationToken);

        if (permissions.Count == 0)
        {
            logger.LogWarning("Bảng Permissions đang trống, bỏ qua seed role. Hãy chạy migration trước.");
            return;
        }

        var permissionById = permissions.ToDictionary(p => p.Id);
        var changed = false;

        foreach (var (roleName, codes) in DesiredPermissions)
        {
            var desiredIds = permissions
                .Where(p => codes.Contains(p.Name))
                .Select(p => p.Id)
                .ToHashSet();

            var role = await dbContext.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);

            if (role is null)
            {
                var createResult = Role.Create(roleName);

                if (createResult.IsError)
                {
                    logger.LogError("Tạo role {Role} thất bại: {Error}", roleName, createResult.FirstError.Description);
                    continue;
                }

                role = createResult.Value;
                ((IAggregateRoot)role).ClearDomainEvents();
                dbContext.Roles.Add(role);

                logger.LogInformation("Đã tạo mới role {Role}.", roleName);
            }

            var currentIds = role.Permissions.Select(p => p.Id).ToHashSet();

            foreach (var permissionId in desiredIds.Except(currentIds))
            {
                if (role.AddPermission(permissionById[permissionId]).IsError)
                    continue;

                changed = true;
            }

            foreach (var permissionId in currentIds.Except(desiredIds))
            {
                if (role.RemovePermission(permissionById[permissionId]).IsError)
                    continue;

                changed = true;
            }

            logger.LogInformation(
                "Role {Role} đang có {Granted}/{Expected} quyền.",
                roleName,
                role.Permissions.Count,
                desiredIds.Count);
        }

        if (!changed)
        {
            logger.LogInformation("Role đã đúng dữ liệu, không có thay đổi nào.");
            return;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Seed role ban đầu hoàn tất.");
    }
}