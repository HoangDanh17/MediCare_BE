using MediatR;
using MediCare.Application.Permissions.Queries.GetAllPermission;
using MediCare.Domain.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.Api.Controllers;

[Route("api/permissions")]
public sealed class PermissionsController(ISender mediator) : ApiController
{
    [HttpGet]
    [Authorize(Policy = PermissionCatalog.PermissionRead)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllPermissionQuery(), ct);
        return Ok(result);
    }
}
