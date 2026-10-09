using ErrorOr;

namespace MediCare.Domain.Permissions;

public static class PermissionErrors
{
    public static Error NotFound => Error.NotFound(
    code: "Permission.NotFound",
    description: "Không tìm thấy quyền.");
}
