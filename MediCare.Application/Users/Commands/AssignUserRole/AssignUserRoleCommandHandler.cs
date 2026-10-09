using ErrorOr;
using MediatR;
using MediCare.Domain.Repositories;
using MediCare.Domain.Roles;
using MediCare.Domain.Users;

namespace MediCare.Application.Users.Commands.AssignUserRole;

public sealed class AssignUserRoleCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository) : IRequestHandler<AssignUserRoleCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        var roleId = new RoleId(request.RoleId);

        var roleExists = await roleRepository.ExistsAsync(roleId, cancellationToken);
        if (!roleExists)
            return RoleErrors.NotFound;

        var user = await userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);
        if (user is null)
            return UserErrors.NotFound;

        if (request.UserId == request.ActorUserId && user.RoleId != roleId)
            return UserErrors.CannotChangeOwnRole;

        var adminRole = await roleRepository.GetByNameAsync(RoleNames.Admin, cancellationToken);
        var isAdminBeingDemoted = adminRole is not null
            && user.RoleId == adminRole.Id
            && roleId != adminRole.Id;

        if (isAdminBeingDemoted
            && !await userRepository.ExistsRoleInUseAsync(adminRole!.Id, user.Id, cancellationToken))
            return UserErrors.LastAdminDemotion;

        var assigned = user.AssignRole(roleId);
        if (assigned.IsError)
            return assigned.Errors;

        return Result.Success;
    }
}
