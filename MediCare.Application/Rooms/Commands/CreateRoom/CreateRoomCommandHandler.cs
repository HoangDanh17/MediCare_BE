using ErrorOr;
using MediatR;
using MediCare.Contract.Room;
using MediCare.Domain.Repositories;
using MediCare.Domain.Rooms;

namespace MediCare.Application.Rooms.Commands.CreateRoom;

public sealed class CreateRoomCommandHandler(
    IRoomRepository roomRepository) : IRequestHandler<CreateRoomCommand, ErrorOr<RoomResponse>>
{
    public async Task<ErrorOr<RoomResponse>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var existingRoom = await roomRepository.GetByNameAsync(request.Name.Trim(), cancellationToken);
        if (existingRoom is not null)
        {
            return RoomErrors.NameAlreadyExists;
        }

        var room = Room.Create(request.Name, request.Type);

        if (room.IsError)
        {
            return room.Errors;
        }

        await roomRepository.AddAsync(room.Value, cancellationToken);

        return RoomResponse.From(room.Value);
    }
}
