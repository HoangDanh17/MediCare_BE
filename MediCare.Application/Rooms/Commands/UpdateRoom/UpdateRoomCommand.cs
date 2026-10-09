using MediCare.Application.Common.Messaging;
using MediCare.Contract.Room;
using MediCare.Domain.Rooms;

namespace MediCare.Application.Rooms.Commands.UpdateRoom;

public sealed record UpdateRoomCommand(RoomId Id, string Name, RoomType Type) : ICommand<RoomResponse>;
