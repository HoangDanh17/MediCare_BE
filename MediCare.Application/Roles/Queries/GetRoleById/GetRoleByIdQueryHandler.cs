using ErrorOr;
using MediatR;
using MediCare.Contract.Role;
using MediCare.Domain.Repositories;
using MediCare.Domain.Roles;

namespace MediCare.Application.Roles.Queries.GetRoleById;

public sealed class GetRoleByIdQueryHandler(
    IRoleRepository roleRepository) : IRequestHandler<GetRoleByIdQuery, ErrorOr<RoleResponse>>
{
    public async Task<ErrorOr<RoleResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(new RoleId(request.Id), cancellationToken);
        if (role is null)
            return RoleErrors.NotFound;

        return new RoleResponse(role.Id.Value,
            role.Name,
            [.. role.Permissions.Select(x => new PermissionResponse(x.Id.Value, x.Name))]);
    }
}
