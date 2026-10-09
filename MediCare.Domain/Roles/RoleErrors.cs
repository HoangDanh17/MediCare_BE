using ErrorOr;

namespace MediCare.Domain.Roles;

public static class RoleErrors
{
    public static Error InvalidName => Error.Validation(
        code: "Role.InvalidName",
        description: "Tên role không được để trống");

    public static Error NotFound => Error.NotFound(
        code: "Role.NotFound",
        description: "Role không tồn tại");

    public static Error AlreadyExists => Error.Conflict(
        code: "Role.AlreadyExists",
        description: "Role đã tồn tại");

    public static Error RoleInUse => Error.Conflict(
        code: "Role.RoleInUse",
        description: "Role đang được sử dụng bởi người dùng khác");

    public static Error PermissionAlreadyGranted => Error.Conflict(
        code: "Role.PermissionAlreadyGranted",
        description: "Quyền đã được cấp cho role này");

    public static Error PermissionNotGranted => Error.NotFound(
        code: "Role.PermissionNotGranted",
        description: "Quyền chưa được cấp cho role này");
    public static Error SomePermissionsNotFound => Error.NotFound(
        code: "Role.SomePermissionsNotFound",
        description: "Một số quyền không tồn tại");

    public static Error SystemRole => Error.Conflict(
        code: "Role.SystemRole",
        description: "Role hệ thống không thể sửa hoặc xóa.");
}
