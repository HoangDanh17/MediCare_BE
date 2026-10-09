using MediCare.Domain.Common;

namespace MediCare.Domain.Majors.Events;

public sealed record MajorDeletedDomainEvent(MajorId Id, DateTime OccurredOnUtc) : IDomainEvent
{
    public string EventType => MajorEventTypes.MajorDeleted;
}
