using MediCare.Domain.Common;
using MediCare.Domain.Permissions;

namespace MediCare.Domain.Users.Events;

public sealed record class UserPermissionGrantedDomainEvent
    (UserId UserId, PermissionId PermissionId, bool IsException, DateTime OccurredOnUtc) : IDomainEvent
{
    public string EventType => UserEventTypes.UserPermissionGranted;
}
