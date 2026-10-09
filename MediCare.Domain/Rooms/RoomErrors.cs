using ErrorOr;

namespace MediCare.Domain.Rooms;

public static class RoomErrors
{
    public static Error InvalidName => Error.Validation(
        code: "Room.InvalidName",
        description: "Tên phòng không được để trống."
    );
    public static Error InvalidRoomType => Error.Validation(
        code: "Room.InvalidRoomType",
        description: "Loại phòng không hợp lệ."
    );
    public static Error NotFound => Error.NotFound(
        code: "Room.NotFound",
        description: "Không tìm thấy phòng."
    );
    public static Error NameAlreadyExists => Error.Conflict(
        code: "Room.NameAlreadyExists",
        description: "Tên phòng đã tồn tại."
    );
    public static Error HasUpcomingSchedules => Error.Conflict(
        code: "Room.HasUpcomingSchedules",
        description: "Phòng còn lịch làm việc sắp tới, không thể ngừng hoạt động."
    );
}
