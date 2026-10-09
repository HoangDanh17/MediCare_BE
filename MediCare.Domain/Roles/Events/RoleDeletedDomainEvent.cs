using MediCare.Domain.Common;

namespace MediCare.Domain.Roles.Events;

public sealed record class RoleDeletedDomainEvent(RoleId Id, DateTime OccurredOnUtc) : IDomainEvent
{
    public string EventType => RoleEventTypes.RoleDeleted;
}
