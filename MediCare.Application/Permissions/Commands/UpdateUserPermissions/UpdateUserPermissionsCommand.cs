using ErrorOr;
using MediCare.Application.Common.Messaging;

namespace MediCare.Application.Permissions.Commands.UpdateUserPermissions;

public sealed record PermissionChangeItem(Guid PermissionId, string Effect);

public sealed record UpdateUserPermissionsCommand(Guid UserId, List<PermissionChangeItem> Changes)
    : ICommand<Success>;