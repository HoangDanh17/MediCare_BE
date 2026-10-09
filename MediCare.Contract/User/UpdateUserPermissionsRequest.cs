namespace MediCare.Contract.User;

public sealed record PermissionChangeRequest(Guid PermissionId, string Effect);

public sealed record UpdateUserPermissionsRequest(List<PermissionChangeRequest> Changes);