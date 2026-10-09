using MediCare.Domain.Roles;

namespace MediCare.Contract.Role;

public record RoleResponse(
    Guid Id,
    string Name,
    List<PermissionResponse> Permissions
)
{
    public bool IsSystem => Name is RoleNames.Admin or RoleNames.User;
}
public record PermissionResponse(
    Guid Id,
    string Name
);