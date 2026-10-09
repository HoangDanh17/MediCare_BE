using MediCare.Domain.Common;

namespace MediCare.Domain.Rooms.Events;

public sealed record RoomUpdatedDomainEvent(RoomId Id, string Name, RoomType Type, DateTime OccurredOnUtc) : IDomainEvent
{
    public string EventType => RoomEventTypes.RoomUpdated;
}
