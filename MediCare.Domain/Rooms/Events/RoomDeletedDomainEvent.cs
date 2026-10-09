using MediCare.Domain.Common;

namespace MediCare.Domain.Rooms.Events;

public sealed record class RoomDeletedDomainEvent(RoomId Id, DateTime OccurredOnUtc) : IDomainEvent
{
    public string EventType => RoomEventTypes.RoomDeleted;
}
