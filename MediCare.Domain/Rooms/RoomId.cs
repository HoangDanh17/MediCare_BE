namespace MediCare.Domain.Rooms;

public readonly record struct RoomId(Guid Value)
{
    public static RoomId New() => new(Guid.NewGuid());
}
