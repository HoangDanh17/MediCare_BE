using ErrorOr;
using MediatR;
using MediCare.Contract.Room;
using MediCare.Domain.Repositories;
using MediCare.Domain.Rooms;

namespace MediCare.Application.Rooms.Commands.ChangeRoomStatus;

public sealed class ChangeRoomStatusCommandHandler(
    IRoomRepository roomRepository) : IRequestHandler<ChangeRoomStatusCommand, ErrorOr<RoomResponse>>
{
    public async Task<ErrorOr<RoomResponse>> Handle(ChangeRoomStatusCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetByIdAsync(request.Id, cancellationToken);

        if (room is null)
        {
            return RoomErrors.NotFound;
        }

        if (request.IsActive)
            room.Activate();
        else
            room.Deactivate();

        return RoomResponse.From(room);
    }
}
