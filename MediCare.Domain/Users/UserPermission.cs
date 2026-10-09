using MediCare.Domain.Permissions;

namespace MediCare.Domain.Users;

public sealed class UserPermission
{
    public UserId UserId { get; private set; }
    public PermissionId PermissionId { get; private set; }
    public bool IsGranted { get; private set; }

    private UserPermission() { }

    public UserPermission(UserId userId, PermissionId permissionId, bool isGranted)
    {
        UserId = userId;
        PermissionId = permissionId;
        IsGranted = isGranted;
    }

    public void SetGranted(bool isGranted) => IsGranted = isGranted;
}
