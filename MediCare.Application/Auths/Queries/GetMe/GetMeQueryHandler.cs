using ErrorOr;
using MediatR;
using MediCare.Contract.User;
using MediCare.Domain.Repositories;
using MediCare.Domain.Users;

namespace MediCare.Application.Auths.Queries.GetMe;

public sealed class GetMeQueryHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository
    ) : IRequestHandler<GetMeQuery, ErrorOr<UserResponse>>
{
    public async Task<ErrorOr<UserResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);

        if (user is null)
            return UserErrors.NotFound;

        var role = await roleRepository.GetByIdAsync(user.RoleId, cancellationToken);

        return UserResponse.From(user, role?.Name ?? string.Empty);
    }
}
