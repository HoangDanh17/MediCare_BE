using ErrorOr;
using MediatR;
using MediCare.Contract.Room;
using MediCare.Domain.Repositories;
using MediCare.Domain.Rooms;

namespace MediCare.Application.Rooms.Queries.GetRoomById;

public sealed class GetRoomByIdQueryHandler(
    IRoomRepository roomRepository) : IRequestHandler<GetRoomByIdQuery, ErrorOr<RoomResponse>>
{
    public async Task<ErrorOr<RoomResponse>> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetByIdAsync(new RoomId(request.Id), cancellationToken);

        if (room is null)
            return RoomErrors.NotFound;

        return RoomResponse.From(room);
    }
}
