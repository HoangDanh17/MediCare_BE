using FluentValidation;

namespace MediCare.Application.Majors.Commands.UpdateMajor;

public sealed class UpdateMajorCommandValidator : AbstractValidator<UpdateMajorCommand>
{
    public UpdateMajorCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên khoa không được để trống.")
            .MaximumLength(100).WithMessage("Tên khoa không được vượt quá 100 ký tự.");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự.");
    }
}
