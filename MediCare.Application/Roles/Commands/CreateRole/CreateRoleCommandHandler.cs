using ErrorOr;
using MediatR;
using MediCare.Contract.Role;
using MediCare.Domain.Repositories;
using MediCare.Domain.Roles;

namespace MediCare.Application.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandHandler(
    IRoleRepository roleRepository,
    IPermissionRepository permissionRepository
    ) : IRequestHandler<CreateRoleCommand, ErrorOr<RoleResponse>>
{
    public async Task<ErrorOr<RoleResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var existingRole = await roleRepository.GetByNameAsync(request.Name, cancellationToken);

        if (existingRole is not null)
            return RoleErrors.AlreadyExists;

        var requestedIds = request.PermissionIds.Distinct().ToList();

        var permissions = await permissionRepository.GetByIdsAsync(requestedIds, cancellationToken);

        if (permissions.Count != requestedIds.Count)
            return RoleErrors.SomePermissionsNotFound;

        var roleResult = Role.Create(request.Name);

        if (roleResult.IsError)
        {
            return roleResult.Errors;
        }

        var role = roleResult.Value;


        foreach (var permission in permissions)
        {
            var result = role.AddPermission(permission);

            if (result.IsError)
                return result.Errors;
        }

        await roleRepository.AddAsync(role, cancellationToken);

        return new RoleResponse(
            role.Id.Value,
            role.Name,
            role.Permissions
                .Select(x => new PermissionResponse(x.Id.Value, x.Name))
                .ToList()
        );
    }
}
