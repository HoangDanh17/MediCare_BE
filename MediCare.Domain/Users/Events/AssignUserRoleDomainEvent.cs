using MediCare.Domain.Common;
using MediCare.Domain.Roles;

namespace MediCare.Domain.Users.Events;

public sealed record AssignUserRoleDomainEvent
    (UserId UserId, Guid PreviousRoleId, RoleId RoleId, bool HadOverrides, DateTime OccurredOnUtc) : IDomainEvent
{
    public string EventType => UserEventTypes.AssignRoleUser;

}
