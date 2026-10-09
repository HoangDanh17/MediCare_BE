using MediatR;
using MediCare.Application.Roles.Commands.CreateRole;
using MediCare.Application.Roles.Commands.DeleteRole;
using MediCare.Application.Roles.Commands.UpdateRole;
using MediCare.Application.Roles.Queries.GetAllRole;
using MediCare.Application.Roles.Queries.GetRoleById;
using MediCare.Contract.Role;
using MediCare.Domain.Permissions;
using MediCare.Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.Api.Controllers;

[Route("api/roles")]
public sealed class RolesController(ISender mediator) : ApiController
{
    [HttpGet]
    [Authorize(Policy = PermissionCatalog.RoleRead)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllRoleQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = PermissionCatalog.RoleRead)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetRoleByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = PermissionCatalog.RoleCreate)]
    public async Task<IActionResult> Create(CreateRoleRequest request, CancellationToken ct)
    {
        var command = new CreateRoleCommand(
            request.Name,
            request.PermissionIds.Select(id => new PermissionId(id)).ToList());

        var result = await mediator.Send(command, ct);

        return result.Match(
            value => StatusCode(StatusCodes.Status201Created, value),
            errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PermissionCatalog.RoleUpdate)]
    public async Task<IActionResult> Update(Guid id, UpdateRoleRequest request, CancellationToken ct)
    {
        var command = new UpdateRoleCommand(
            new RoleId(id),
            request.Name,
            request.PermissionIds.Select(pid => new PermissionId(pid)).ToList());

        var result = await mediator.Send(command, ct);

        return result.Match(value => Ok(value), errors => Problem(errors));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PermissionCatalog.RoleDelete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteRoleCommand(id), ct);
        return result.Match(_ => NoContent(), errors => Problem(errors));
    }
}
