using ErrorOr;
using MediatR;
using MediCare.Application.Common.Messaging;
using MediCare.Contract.Common;
using MediCare.Contract.User;
using MediCare.Domain.Repositories;

namespace MediCare.Application.Users.Queries.GetAllUsers;

public sealed class GetAllUsersQueryHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository
    ) : IRequestHandler<GetAllUsersQuery, ErrorOr<PagedResponse<UserResponse>>>
{
    public async Task<ErrorOr<PagedResponse<UserResponse>>> Handle(
        GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var users = await userRepository.GetPageAsync(page, pageSize, request.Search, cancellationToken);
        var totalCount = await userRepository.CountAsync(request.Search, cancellationToken);

        var roles = await roleRepository.GetAllAsync(cancellationToken);
        var roleNames = roles.ToDictionary(r => r.Id.Value, r => r.Name);

        var items = users
            .Select(u => UserResponse.From(u, roleNames.GetValueOrDefault(u.RoleId.Value, string.Empty)))
            .ToList();

        return new PagedResponse<UserResponse>(page, pageSize, totalCount, items);
    }
}
