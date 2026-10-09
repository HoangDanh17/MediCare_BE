using FluentValidation;

namespace MediCare.Application.Roles.Commands.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên vai trò là bắt buộc.")
            .MaximumLength(100).WithMessage("Tên vai trò tối đa 100 ký tự.");
    }
}
