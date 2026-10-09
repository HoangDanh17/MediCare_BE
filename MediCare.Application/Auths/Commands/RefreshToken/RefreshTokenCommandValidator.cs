using FluentValidation;

namespace MediCare.Application.Auths.Commands.RefreshToken;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator() =>
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token là bắt buộc.")
            .MaximumLength(200).WithMessage("Refresh token tối đa 200 ký tự.");
}
