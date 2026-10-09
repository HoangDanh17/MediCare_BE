namespace MediCare.Domain.Roles.Events;

public static class RoleEventTypes
{
    public const string RoleCreated = "Role.Created";
    public const string RoleUpdated = "Role.Updated";
    public const string RoleDeleted = "Role.Deleted";
    public const string RolePermissionGranted = "Role.PermissionGranted";
    public const string RolePermissionRemoved = "Role.PermissionRemoved";
}
