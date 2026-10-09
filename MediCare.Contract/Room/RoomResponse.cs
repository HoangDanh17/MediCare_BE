using MediCare.Domain.Rooms;
using DomainRoom = MediCare.Domain.Rooms.Room;

namespace MediCare.Contract.Room;

public sealed record RoomResponse(
    Guid Id,
    string Name,
    RoomType Type,
    bool IsActive)
{
    public static RoomResponse From(DomainRoom room) =>
        new(room.Id.Value, room.Name, room.Type, room.IsActive);
}
