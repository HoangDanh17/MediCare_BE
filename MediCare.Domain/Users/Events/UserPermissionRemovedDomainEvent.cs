using MediCare.Domain.Common;
using MediCare.Domain.Permissions;

namespace MediCare.Domain.Users.Events;

public sealed record class UserPermissionRemovedDomainEvent
    (UserId UserId, PermissionId PermissionId, DateTime OccurredOnUtc) : IDomainEvent
{
    public string EventType => UserEventTypes.UserPermissionRemoved;
}
