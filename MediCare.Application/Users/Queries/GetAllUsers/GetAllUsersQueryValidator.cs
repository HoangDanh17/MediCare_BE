using FluentValidation;

namespace MediCare.Application.Users.Queries.GetAllUsers;

public sealed class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
    public GetAllUsersQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Trang phải lớn hơn hoặc bằng 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("pageSize phải từ 1 đến 100.");

        RuleFor(x => x.Search)
            .MaximumLength(256).WithMessage("Từ khóa tìm kiếm tối đa 256 ký tự.");
    }
}
