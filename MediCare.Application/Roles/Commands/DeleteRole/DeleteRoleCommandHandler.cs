using ErrorOr;
using MediatR;
using MediCare.Domain.Repositories;
using MediCare.Domain.Roles;

namespace MediCare.Application.Roles.Commands.DeleteRole;

public sealed class DeleteRoleCommandHandler(
    IRoleRepository roleRepository,
    IUserRepository userRepository
) : IRequestHandler<DeleteRoleCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var roleId = new RoleId(request.Id);

        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role is null)
            return RoleErrors.NotFound;

        if (role.Name is RoleNames.Admin or RoleNames.User)
            return RoleErrors.SystemRole;

        if (await userRepository.ExistsRoleInUseAsync(roleId, cancellationToken))
            return RoleErrors.RoleInUse;

        await roleRepository.DeleteAsync(role, cancellationToken);

        return Result.Success;
    }
}
