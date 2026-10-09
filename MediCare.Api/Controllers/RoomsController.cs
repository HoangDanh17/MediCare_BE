using MediatR;
using MediCare.Application.Rooms.Commands.ChangeRoomStatus;
using MediCare.Application.Rooms.Commands.CreateRoom;
using MediCare.Application.Rooms.Commands.DeleteRoom;
using MediCare.Application.Rooms.Commands.UpdateRoom;
using MediCare.Application.Rooms.Queries.GetAllRooms;
using MediCare.Application.Rooms.Queries.GetRoomById;
using MediCare.Contract.Room;
using MediCare.Domain.Permissions;
using MediCare.Domain.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.Api.Controllers;

[Route("api/rooms")]
public sealed class RoomsController(ISender mediator) : ApiController
{
    [HttpGet]
    [Authorize(Policy = PermissionCatalog.RoomRead)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllRoomsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = PermissionCatalog.RoomRead)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetRoomByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = PermissionCatalog.RoomCreate)]
    public async Task<IActionResult> Create(CreateRoomRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateRoomCommand(request.Name, request.Type), ct);

        return result.Match(
            value => StatusCode(StatusCodes.Status201Created, value),
            errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PermissionCatalog.RoomUpdate)]
    public async Task<IActionResult> Update(Guid id, UpdateRoomRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(
            new UpdateRoomCommand(new RoomId(id), request.Name, request.Type), ct);

        return result.Match(value => Ok(value), errors => Problem(errors));
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Policy = PermissionCatalog.RoomUpdate)]
    public async Task<IActionResult> ChangeStatus(Guid id, ChangeRoomStatusRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new ChangeRoomStatusCommand(new RoomId(id), request.IsActive), ct);
        return result.Match(value => Ok(value), errors => Problem(errors));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PermissionCatalog.RoomDelete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteRoomCommand(new RoomId(id)), ct);
        return result.Match(_ => NoContent(), errors => Problem(errors));
    }
}
