using MediCare.Application.Common.Messaging;
using MediCare.Contract.Room;

namespace MediCare.Application.Rooms.Queries.GetRoomById;

public sealed record GetRoomByIdQuery(Guid Id) : IQuery<RoomResponse>;
