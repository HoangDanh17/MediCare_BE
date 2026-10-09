using FluentValidation;
using MediCare.Domain.Users;

namespace MediCare.Application.Permissions.Commands.UpdateUserPermissions;

public sealed class UpdateUserPermissionsCommandValidator : AbstractValidator<UpdateUserPermissionsCommand>
{
    public UpdateUserPermissionsCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.Changes)
            .NotEmpty().WithMessage("Cần ít nhất một thay đổi.")
            .Must(changes => changes.Select(c => c.PermissionId).Distinct().Count() == changes.Count)
            .WithMessage("Một quyền chỉ được xuất hiện một lần.");

        RuleForEach(x => x.Changes).ChildRules(change =>
        {
            change.RuleFor(c => c.PermissionId).NotEmpty();
            change.RuleFor(c => c.Effect)
                .Must(IsValidEffect)
                .WithMessage("Effect phải là Inherit, Allow hoặc Deny.");
        });
    }

    private static bool IsValidEffect(string? value) =>
        Enum.TryParse<PermissionEffect>(value, ignoreCase: true, out var effect)
        && Enum.IsDefined(effect);
}