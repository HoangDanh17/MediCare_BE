using ErrorOr;
using MediCare.Domain.Common;
using MediCare.Domain.Rooms.Events;

namespace MediCare.Domain.Rooms;

public sealed class Room : AggregateRoot<RoomId>
{
    public string Name { get; private set; } = null!;
    public RoomType Type { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Room() { }
    private Room(RoomId id, string name, RoomType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    public static ErrorOr<Room> Create(string name, RoomType type)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoomErrors.InvalidName;

        if (!Enum.IsDefined(type))
            return RoomErrors.InvalidRoomType;

        var roomId = RoomId.New();

        var room = new Room(roomId, name.Trim(), type);

        // Raise a domain event for room creation
        room.RaiseDomainEvent(new RoomCreatedDomainEvent(room.Id, room.Name, room.Type, DateTime.Now));
        return room;
    }

    public ErrorOr<Success> Update(string name, RoomType type)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoomErrors.InvalidName;

        if (!Enum.IsDefined(type))
            return RoomErrors.InvalidRoomType;

        var trimmed = name.Trim();

        // Không đổi gì thì không phát event
        if (trimmed == Name && type == Type)
            return Result.Success;

        Name = trimmed;
        Type = type;

        RaiseDomainEvent(new RoomUpdatedDomainEvent(Id, Name, Type, DateTime.Now));

        return Result.Success;
    }

    public void Delete()
    {
        RaiseDomainEvent(new RoomDeletedDomainEvent(Id, DateTime.Now));
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;

        RaiseDomainEvent(new RoomStatusChangedDomainEvent(Id, IsActive, DateTime.Now));
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;

        RaiseDomainEvent(new RoomStatusChangedDomainEvent(Id, IsActive, DateTime.Now));
    }

}
