namespace MediCare.Contract.Role;

public sealed record CreateRoleRequest(string Name, List<Guid> PermissionIds);

public sealed record UpdateRoleRequest(string Name, List<Guid> PermissionIds);
