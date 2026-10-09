using MediCare.Domain.Common;
using MediCare.Domain.Permissions;

namespace MediCare.Domain.Roles.Events;

public sealed record class RolePermissionRemovedDomainEvent
    (RoleId RoleId, PermissionId PermissionId, DateTime OccurredOnUtc) : IDomainEvent
{
    public string EventType => RoleEventTypes.RolePermissionRemoved;
}
