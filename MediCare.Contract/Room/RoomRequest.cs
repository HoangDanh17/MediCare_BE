using MediCare.Domain.Rooms;

namespace MediCare.Contract.Room;

public sealed record CreateRoomRequest(string Name, RoomType Type);

public sealed record UpdateRoomRequest(string Name, RoomType Type);

public sealed record ChangeRoomStatusRequest(bool IsActive);
