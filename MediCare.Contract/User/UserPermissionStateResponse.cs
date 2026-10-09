namespace MediCare.Contract.User;

public sealed record UserPermissionStateResponse(
 Guid Id,
 string Name,
 string DisplayName,
 bool FromRole,
 string Setting,
 bool HasPermission);
