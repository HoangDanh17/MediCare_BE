using System.Security.Cryptography;
using System.Text;

namespace MediCare.Domain.Permissions;

public static class PermissionCatalog
{
    public const string UserRead = "user.read";
    public const string UserCreate = "user.create";
    public const string UserUpdate = "user.update";
    public const string UserDelete = "user.delete";

    public const string RoleRead = "role.read";
    public const string RoleCreate = "role.create";
    public const string RoleUpdate = "role.update";
    public const string RoleDelete = "role.delete";

    public const string PermissionRead = "permission.read";

    public const string MajorRead = "major.read";
    public const string MajorCreate = "major.create";
    public const string MajorUpdate = "major.update";
    public const string MajorDelete = "major.delete";

    public const string RoomRead = "room.read";
    public const string RoomCreate = "room.create";
    public const string RoomUpdate = "room.update";
    public const string RoomDelete = "room.delete";

    public static readonly IReadOnlyDictionary<string, string> DisplayNames =
        new Dictionary<string, string>
        {
            [UserRead] = "Xem người dùng",
            [UserCreate] = "Tạo người dùng",
            [UserUpdate] = "Cập nhật người dùng",
            [UserDelete] = "Xóa người dùng",

            [RoleRead] = "Xem vai trò",
            [RoleCreate] = "Tạo vai trò",
            [RoleUpdate] = "Cập nhật vai trò",
            [RoleDelete] = "Xóa vai trò",

            [PermissionRead] = "Xem quyền",

            [MajorRead] = "Xem khoa",
            [MajorCreate] = "Tạo khoa",
            [MajorUpdate] = "Cập nhật khoa",
            [MajorDelete] = "Xóa khoa",

            [RoomRead] = "Xem phòng",
            [RoomCreate] = "Tạo phòng",
            [RoomUpdate] = "Cập nhật phòng",
            [RoomDelete] = "Xóa phòng",
        };

    public static IEnumerable<string> Codes => DisplayNames.Keys;

    public static Guid IdOf(string code)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(code));
        return new Guid(hash.AsSpan(0, 16));
    }
}