using MediCare.Domain.Common;

namespace MediCare.Domain.Rooms.Events;

public sealed record RoomStatusChangedDomainEvent(RoomId Id, bool IsActive, DateTime OccurredOnUtc) : IDomainEvent
{
    public string EventType => RoomEventTypes.RoomStatusChanged;
}
