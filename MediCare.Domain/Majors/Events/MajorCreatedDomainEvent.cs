using MediCare.Domain.Common;

namespace MediCare.Domain.Majors.Events;

public sealed record MajorCreatedDomainEvent(MajorId Id, string Name, string? Description, DateTime OccurredOnUtc) : IDomainEvent
{
    public string EventType => MajorEventTypes.MajorCreated;
}
