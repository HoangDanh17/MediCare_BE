using MediatR;
using MediCare.Application.Majors.Commands.CreateMajor;
using MediCare.Application.Majors.Commands.DeleteMajor;
using MediCare.Application.Majors.Commands.UpdateMajor;
using MediCare.Application.Majors.Queries.GetAllMajors;
using MediCare.Application.Majors.Queries.GetMajorById;
using MediCare.Contract.Major;
using MediCare.Domain.Majors;
using MediCare.Domain.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.Api.Controllers;

[Route("api/majors")]
public sealed class MajorsController(ISender mediator) : ApiController
{
    [HttpGet]
    [Authorize(Policy = PermissionCatalog.MajorRead)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllMajorsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = PermissionCatalog.MajorRead)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetMajorByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = PermissionCatalog.MajorCreate)]
    public async Task<IActionResult> Create(CreateMajorRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateMajorCommand(request.Name, request.Description), ct);

        return result.Match(
            value => StatusCode(StatusCodes.Status201Created, value),
            errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PermissionCatalog.MajorUpdate)]
    public async Task<IActionResult> Update(Guid id, UpdateMajorRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(
            new UpdateMajorCommand(new MajorId(id), request.Name, request.Description), ct);

        return result.Match(value => Ok(value), errors => Problem(errors));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PermissionCatalog.MajorDelete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteMajorCommand(new MajorId(id)), ct);
        return result.Match(_ => NoContent(), errors => Problem(errors));
    }
}
