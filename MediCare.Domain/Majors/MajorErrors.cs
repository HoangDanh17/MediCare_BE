using ErrorOr;

namespace MediCare.Domain.Majors;

public static class MajorErrors
{
    public static Error InvalidName => Error.Validation(
        code: "Major.InvalidName",
        description: "Tên khoa không được để trống."
    );
    public static Error AlreadyExist => Error.Conflict(
        code: "Major.AlreadyExists",
        description: "Tên khoa đã tồn tại."
    );
    public static Error NotFound => Error.NotFound(
        code: "Major.NotFound",
        description: "Khoa không tồn tại."
    );
}
