using ErrorOr;
using MediatR;
using MediCare.Contract.Auth;

namespace MediCare.Application.Auths.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<ErrorOr<TokenResponse>>;
