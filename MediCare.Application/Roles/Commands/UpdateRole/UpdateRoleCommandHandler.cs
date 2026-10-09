using ErrorOr;
using MediatR;
using MediCare.Contract.Role;
using MediCare.Domain.Repositories;
using MediCare.Domain.Roles;

namespace MediCare.Application.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommandHandler(
    IRoleRepository roleRepository,
    IPermissionRepository permissionRepository
) : IRequestHandler<UpdateRoleCommand, ErrorOr<RoleResponse>>
{
    public async Task<ErrorOr<RoleResponse>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (role is null)
        {
            return RoleErrors.NotFound;
        }

        if (role.Name is RoleNames.Admin or RoleNames.User)
            return RoleErrors.SystemRole;

        var duplicateName = await roleRepository.GetByNameAsync(request.Name, cancellationToken);
        if (duplicateName is not null && duplicateName.Id != role.Id)
            return RoleErrors.AlreadyExists;

        // lay list permission id distinct de tranh truong hop truyen trung lap permission id
        var requestedIds = request.PermissionIds.Distinct().ToList();

        var permissions = await permissionRepository.GetByIdsAsync(requestedIds, cancellationToken);

        if (permissions.Count != requestedIds.Count)
        {
            return RoleErrors.SomePermissionsNotFound;
        }

        var updateResult = role.Update(request.Name);

        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        // remove permission that is not in the requested list
        var desiredIds = permissions.Select(p => p.Id).ToHashSet();

        foreach (var permission in role.Permissions.ToList())
        {
            if (!desiredIds.Contains(permission.Id))
                role.RemovePermission(permission);
        }

        foreach (var permission in permissions)
        {
            if (role.Permissions.Any(p => p.Id == permission.Id))
                continue;

            var result = role.AddPermission(permission);

            if (result.IsError)
                return result.Errors;
        }

        return new RoleResponse(
            role.Id.Value,
            role.Name,
            role.Permissions
                .Select(x => new PermissionResponse(x.Id.Value, x.Name))
                .ToList()
        );
    }
}
