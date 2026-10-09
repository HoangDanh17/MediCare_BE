using FluentValidation;

namespace MediCare.Application.Rooms.Commands.UpdateRoom;

public sealed class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên phòng không được để trống.")
            .MaximumLength(100).WithMessage("Tên phòng không được vượt quá 100 ký tự.");
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Loại phòng không hợp lệ.");
    }
}
