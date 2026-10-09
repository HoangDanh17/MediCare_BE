using ErrorOr;
using MediatR;
using MediCare.Contract.Role;
using MediCare.Domain.Repositories;

namespace MediCare.Application.Roles.Queries.GetAllRole;

public sealed class GetAllRoleQueryHandler(IRoleRepository roleRepository) : IRequestHandler<GetAllRoleQuery, ErrorOr<IReadOnlyList<RoleResponse>>>
{
    public async Task<ErrorOr<IReadOnlyList<RoleResponse>>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleRepository.GetAllAsync(cancellationToken);

        return roles.Select(role => new RoleResponse(role.Id.Value,
            role.Name,
            [.. role.Permissions.Select(x => new PermissionResponse(x.Id.Value, x.Name))]))
            .ToList();
    }
}
