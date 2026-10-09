using ErrorOr;
using MediCare.Application.Common.Messaging;
using MediCare.Contract.Room;
using MediCare.Domain.Rooms;

namespace MediCare.Application.Rooms.Commands.ChangeRoomStatus;

public sealed record ChangeRoomStatusCommand(RoomId Id, bool IsActive) : ICommand<RoomResponse>;
