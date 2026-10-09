using MediCare.Domain.Common;

namespace MediCare.Domain.Rooms.Events;

public sealed record RoomCreatedDomainEvent(RoomId Id, string Name, RoomType Type, DateTime OccurredOnUtc) : IDomainEvent
{
    public string EventType => RoomEventTypes.RoomCreated;
}
