using FluentValidation;

namespace MediCare.Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email là bắt buộc.")
            .EmailAddress().WithMessage("Email không hợp lệ.")
            .MaximumLength(256).WithMessage("Email tối đa 256 ký tự.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu là bắt buộc.")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.")
            .MaximumLength(128).WithMessage("Mật khẩu tối đa 128 ký tự.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ tên là bắt buộc.")
            .MaximumLength(200).WithMessage("Họ tên tối đa 200 ký tự.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Địa chỉ là bắt buộc.")
            .MaximumLength(500).WithMessage("Địa chỉ tối đa 500 ký tự.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Số điện thoại là bắt buộc.")
            .Matches(@"^\+?\d{10,12}$")
            .WithMessage("Số điện thoại không hợp lệ (10-12 chữ số, có thể bắt đầu bằng +).");

        RuleFor(x => x.Dob)
            .Must(dob => dob <= DateTime.UtcNow)
            .WithMessage("Ngày sinh không được là ngày tương lai.");

        RuleFor(x => x.Gender)
            .Must(gender => Enum.IsDefined(gender))
            .WithMessage("Giới tính phải là 0 (Nam) hoặc 1 (Nữ).");
    }
}
