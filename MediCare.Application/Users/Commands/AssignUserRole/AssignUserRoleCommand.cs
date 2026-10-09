using ErrorOr;
using MediCare.Application.Common.Messaging;

namespace MediCare.Application.Users.Commands.AssignUserRole;

public sealed record AssignUserRoleCommand(
    Guid UserId,
    Guid RoleId,
    Guid ActorUserId
) : ICommand<Success>;
