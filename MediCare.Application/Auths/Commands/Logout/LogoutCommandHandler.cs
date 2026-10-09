using ErrorOr;
using MediatR;
using MediCare.Application.Common.Interfaces;
using MediCare.Domain.Users;

namespace MediCare.Application.Auths.Commands.Logout;

public sealed class LogoutCommandHandler(IRefreshTokenStore refreshTokenStore)
    : IRequestHandler<LogoutCommand, ErrorOr<Success>>,
      IRequestHandler<LogoutAllCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await refreshTokenStore.RevokeAsync(request.RefreshToken, cancellationToken);
        return Result.Success;
    }

    public async Task<ErrorOr<Success>> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
    {
        await refreshTokenStore.RevokeAllAsync(new UserId(request.UserId), cancellationToken);
        return Result.Success;
    }
}