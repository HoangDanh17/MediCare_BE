using ErrorOr;
using MediatR;
using MediCare.Application.Common.Interfaces;
using MediCare.Contract.Auth;
using MediCare.Domain.Repositories;
using MediCare.Domain.Users;

namespace MediCare.Application.Auths.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenStore refreshTokenStore)
    : IRequestHandler<RefreshTokenCommand, ErrorOr<TokenResponse>>
{
    public async Task<ErrorOr<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Dùng một lần: token cũ bị xóa ngay, dùng lại sẽ thu hồi toàn bộ phiên của user
        var consumed = await refreshTokenStore.ConsumeAsync(request.RefreshToken, cancellationToken);
        if (consumed.IsError)
            return consumed.Errors;

        // Nạp lại user để quyền, trạng thái khóa luôn mới
        var user = await userRepository.GetByIdAsync(consumed.Value, cancellationToken);
        if (user is null || !user.IsActive)
            return UserErrors.InvalidRefreshToken;

        var accessToken = jwtTokenGenerator.Generate(user);
        var refreshToken = await refreshTokenStore.IssueAsync(user.Id, cancellationToken);

        return new TokenResponse(accessToken.Value, refreshToken, accessToken.ExpiresAtUtc);
    }
}
