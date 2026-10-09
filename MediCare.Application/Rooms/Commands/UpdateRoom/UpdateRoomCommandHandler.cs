using ErrorOr;
using MediatR;
using MediCare.Contract.Room;
using MediCare.Domain.Repositories;
using MediCare.Domain.Rooms;

namespace MediCare.Application.Rooms.Commands.UpdateRoom;

public sealed class UpdateRoomCommandHandler(
    IRoomRepository roomRepository) : IRequestHandler<UpdateRoomCommand, ErrorOr<RoomResponse>>
{
    public async Task<ErrorOr<RoomResponse>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var existingRoom = await roomRepository.GetByIdAsync(request.Id, cancellationToken);

        if (existingRoom is null)
        {
            return RoomErrors.NotFound;
        }

        var trimmedName = request.Name.Trim();

        var duplicate = await roomRepository.GetByNameAsync(trimmedName, cancellationToken);
        if (duplicate is not null && duplicate.Id != request.Id)
        {
            return RoomErrors.NameAlreadyExists;
        }

        var updateResult = existingRoom.Update(trimmedName, request.Type);

        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        return RoomResponse.From(existingRoom);
    }
}
