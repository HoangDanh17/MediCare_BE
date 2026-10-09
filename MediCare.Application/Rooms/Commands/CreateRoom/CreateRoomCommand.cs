using MediCare.Application.Common.Messaging;
using MediCare.Contract.Room;
using MediCare.Domain.Rooms;

namespace MediCare.Application.Rooms.Commands.CreateRoom;

public sealed record CreateRoomCommand(string Name, RoomType Type) : ICommand<RoomResponse>;
