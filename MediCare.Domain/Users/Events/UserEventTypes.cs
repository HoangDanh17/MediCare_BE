namespace MediCare.Domain.Users.Events;

public static class UserEventTypes
{
    public const string UserCreated = "User.Created";
    public const string UserUpdated = "User.Updated";
    public const string UserDeleted = "User.Deleted";
    public const string AssignRoleUser = "User.AssignRole";
    public const string UserPermissionGranted = "UserPermission.Granted";
    public const string UserPermissionRemoved = "UserPermission.Removed";
}
