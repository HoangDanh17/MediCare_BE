using MediatR;
using MediCare.Application.Auths.Commands.Login;
using MediCare.Application.Auths.Commands.Logout;
using MediCare.Application.Auths.Commands.RefreshToken;
using MediCare.Application.Auths.Queries.GetMe;
using MediCare.Application.Permissions.Queries.GetUserPermissions;
using MediCare.Contract.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.Api.Controllers;

[Route("api/auth")]
public sealed class AuthController(ISender mediator) : ApiController
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new LoginCommand(request.Email, request.Password), ct);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new RefreshTokenCommand(request.RefreshToken), ct);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new LogoutCommand(request.RefreshToken), ct);
        return result.Match(_ => NoContent(), errors => Problem(errors));
    }

    [Authorize]
    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAll(CancellationToken ct)
    {
        if (CurrentUserId is not { } userId)
            return Unauthorized();

        var result = await mediator.Send(new LogoutAllCommand(userId), ct);
        return result.Match(_ => NoContent(), errors => Problem(errors));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        if (CurrentUserId is not { } userId)
            return Unauthorized();

        var result = await mediator.Send(new GetMeQuery(userId), ct);
        return result.Match(value => Ok(value), errors => Problem(errors));
    }

    [Authorize]
    [HttpGet("me/permissions")]
    public async Task<IActionResult> MyPermissions(CancellationToken ct)
    {
        if (CurrentUserId is not { } userId)
            return Unauthorized();

        var result = await mediator.Send(new GetUserPermissionsQuery(userId), ct);
        return Ok(result);
    }
}