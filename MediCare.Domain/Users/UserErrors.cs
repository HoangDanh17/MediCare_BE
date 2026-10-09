using ErrorOr;

namespace MediCare.Domain.Users;

public static class UserErrors
{
    public static Error InvalidFullName => Error.Validation(
        code: "User.InvalidFullName",
        description: "Tên người dùng không được để trống.");

    public static Error InvalidEmail => Error.Validation(
        code: "User.InvalidEmail",
        description: "Email không hợp lệ.");

    public static Error DuplicateEmail => Error.Conflict(
       code: "User.DuplicateEmail",
       description: "Email này đã được sử dụng trên hệ thống.");

    public static Error InvalidPasswordHash => Error.Validation(
        code: "User.InvalidPasswordHash",
        description: "Mật khẩu không hợp lệ.");

    public static Error InvalidPhoneNumber => Error.Validation(
        code: "User.InvalidPhoneNumber",
        description: "Số điện thoại không hợp lệ.");

    public static Error InvalidAddress => Error.Validation(
        code: "User.InvalidAddress",
        description: "Địa chỉ không hợp lệ.");

    public static Error NotFound => Error.NotFound(
        code: "User.NotFound",
        description: "Người dùng không tồn tại.");

    public static Error InvalidRoleId => Error.Validation(
       code: "User.InvalidRoleId",
       description: "Vai trò không hợp lệ.");

    public static Error NotFoundPermission => Error.NotFound(
        code: "UserPermission.NotFound",
        description: "Người dùng không có quyền này.");

    // Auth
    public static Error InvalidCredentials => Error.Validation(
        code: "User.InvalidCredentials",
        description: "Email hoặc mật khẩu không đúng.");

    public static Error InvalidRefreshToken => Error.Validation(
        code: "User.InvalidRefreshToken",
        description: "Refresh token không hợp lệ.");

    public static Error AccountDisabled => Error.Validation(
        code: "User.AccountDisabled",
        description: "Tài khoản đã bị khóa.");

    public static Error CannotChangeOwnRole => Error.Conflict(
        code: "User.CannotChangeOwnRole",
        description: "Bạn không thể thay đổi vai trò của chính mình.");

    public static Error LastAdminDemotion => Error.Conflict(
        code: "User.LastAdminDemotion",
        description: "Không thể hạ vai trò của Admin cuối cùng.");
}
