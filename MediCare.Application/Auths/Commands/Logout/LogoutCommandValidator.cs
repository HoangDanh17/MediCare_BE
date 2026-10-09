using FluentValidation;

namespace MediCare.Application.Auths.Commands.Logout;

public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator() =>
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token là bắt buộc.")
            .MaximumLength(200).WithMessage("Refresh token tối đa 200 ký tự.");
}
