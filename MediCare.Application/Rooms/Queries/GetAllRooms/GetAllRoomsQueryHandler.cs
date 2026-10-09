using ErrorOr;
using MediatR;
using MediCare.Contract.Room;
using MediCare.Domain.Repositories;

namespace MediCare.Application.Rooms.Queries.GetAllRooms;

public sealed class GetAllRoomsQueryHandler(
    IRoomRepository roomRepository) : IRequestHandler<GetAllRoomsQuery, ErrorOr<IReadOnlyList<RoomResponse>>>
{
    public async Task<ErrorOr<IReadOnlyList<RoomResponse>>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetAllAsync(cancellationToken);

        return rooms
            .OrderBy(r => r.Name)
            .Select(RoomResponse.From)
            .ToList();
    }
}
