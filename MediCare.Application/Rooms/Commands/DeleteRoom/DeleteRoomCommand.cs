using ErrorOr;
using MediCare.Application.Common.Messaging;
using MediCare.Domain.Rooms;

namespace MediCare.Application.Rooms.Commands.DeleteRoom;

public sealed record DeleteRoomCommand(RoomId Id) : ICommand<Success>;
