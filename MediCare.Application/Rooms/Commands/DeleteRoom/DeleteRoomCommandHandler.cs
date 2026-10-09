using ErrorOr;
using MediatR;
using MediCare.Domain.Repositories;
using MediCare.Domain.Rooms;

namespace MediCare.Application.Rooms.Commands.DeleteRoom;

public sealed class DeleteRoomCommandHandler(
    IRoomRepository roomRepository) : IRequestHandler<DeleteRoomCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetByIdAsync(request.Id, cancellationToken);

        if (room is null)
        {
            return RoomErrors.NotFound;
        }

        await roomRepository.DeleteAsync(room, cancellationToken);

        return Result.Success;
    }
}
