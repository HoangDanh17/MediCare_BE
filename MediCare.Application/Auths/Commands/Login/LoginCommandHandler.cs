using ErrorOr;
using MediatR;
using MediCare.Application.Common.Interfaces;
using MediCare.Contract.Auth;
using MediCare.Domain.Repositories;
using MediCare.Domain.Users;

namespace MediCare.Application.Auths.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenStore refreshTokenStore)
    : IRequestHandler<LoginCommand, ErrorOr<TokenResponse>>
{
    public async Task<ErrorOr<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return UserErrors.InvalidCredentials;

        if (!user.IsActive)
            return UserErrors.AccountDisabled;

        var accessToken = jwtTokenGenerator.Generate(user);
        var refreshToken = await refreshTokenStore.IssueAsync(user.Id, cancellationToken);

        return new TokenResponse(accessToken.Value, refreshToken, accessToken.ExpiresAtUtc);
    }
}