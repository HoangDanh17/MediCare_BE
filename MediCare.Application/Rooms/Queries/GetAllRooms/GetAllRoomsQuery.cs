using MediCare.Application.Common.Messaging;
using MediCare.Contract.Room;

namespace MediCare.Application.Rooms.Queries.GetAllRooms;

public sealed record GetAllRoomsQuery() : IQuery<IReadOnlyList<RoomResponse>>;
