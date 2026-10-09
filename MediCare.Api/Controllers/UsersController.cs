using MediatR;
using MediCare.Application.Permissions.Commands.UpdateUserPermissions;
using MediCare.Application.Permissions.Queries.GetUserPermissions;
using MediCare.Application.Users.Commands.AssignUserRole;
using MediCare.Application.Users.Commands.CreateUser;
using MediCare.Application.Users.Queries.GetAllUsers;
using MediCare.Application.Users.Queries.GetUserById;
using MediCare.Contract.User;
using MediCare.Domain.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.Api.Controllers;

[Route("api/users")]
public sealed class UsersController(ISender mediator) : ApiController
{
    [HttpGet]
    [Authorize(Policy = PermissionCatalog.UserRead)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllUsersQuery(page, pageSize, search), ct);
        return result.Match(value => Ok(value), errors => Problem(errors));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = PermissionCatalog.UserRead)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id), ct);
        return result.Match(value => Ok(value), errors => Problem(errors));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(CreateUserRequest request, CancellationToken ct)
    {
        var command = new CreateUserCommand(
            request.FullName,
            request.Email,
            request.Password,
            request.PhoneNumber,
            request.Address,
            request.Gender,
            request.Dob);

        var result = await mediator.Send(command, ct);

        return result.Match(
            value => StatusCode(StatusCodes.Status201Created, value),
            errors => Problem(errors));
    }

    [HttpPut("{id:guid}/role")]
    [Authorize(Policy = PermissionCatalog.UserUpdate)]
    public async Task<IActionResult> AssignRole(Guid id, AssignUserRoleRequest request, CancellationToken ct)
    {
        if (CurrentUserId is not { } actorId)
            return Unauthorized();

        var result = await mediator.Send(new AssignUserRoleCommand(id, request.RoleId, actorId), ct);
        return result.Match(_ => NoContent(), errors => Problem(errors));
    }

    [HttpGet("{id:guid}/permissions")]
    [Authorize(Policy = PermissionCatalog.UserRead)]
    public async Task<IActionResult> GetPermissions(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetUserPermissionsQuery(id), ct);
        return Ok(result);
    }

    [HttpPut("{id:guid}/permissions")]
    [Authorize(Policy = PermissionCatalog.UserUpdate)]
    public async Task<IActionResult> UpdatePermissions(
        Guid id, UpdateUserPermissionsRequest request, CancellationToken ct)
    {
        var command = new UpdateUserPermissionsCommand(
            id,
            request.Changes.Select(c => new PermissionChangeItem(c.PermissionId, c.Effect)).ToList());

        var result = await mediator.Send(command, ct);

        return result.Match(_ => NoContent(), errors => Problem(errors));
    }
}
