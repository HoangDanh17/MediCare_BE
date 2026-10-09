using ErrorOr;
using MediatR;
using MediCare.Domain.Permissions;
using MediCare.Domain.Repositories;
using MediCare.Domain.Users;

namespace MediCare.Application.Permissions.Commands.UpdateUserPermissions;

public sealed class UpdateUserPermissionsCommandHandler(
    IUserRepository userRepository,
    IPermissionRepository permissionRepository)
    : IRequestHandler<UpdateUserPermissionsCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(UpdateUserPermissionsCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);
        if (user is null)
            return UserErrors.NotFound;

        var ids = request.Changes.Select(c => new PermissionId(c.PermissionId)).ToList();

        var found = await permissionRepository.GetByIdsAsync(ids, cancellationToken);
        if (found.Count != ids.Count)
            return PermissionErrors.NotFound;   // có id không tồn tại

        foreach (var change in request.Changes)
        {
            var effect = Enum.Parse<PermissionEffect>(change.Effect, ignoreCase: true);
            user.SetPermission(new PermissionId(change.PermissionId), effect);
        }

        return Result.Success;
    }
}
