using ErrorOr;
using MediatR;

namespace MediCare.Application.Auths.Commands.Logout;

public sealed record LogoutCommand(string RefreshToken) : IRequest<ErrorOr<Success>>;

public sealed record LogoutAllCommand(Guid UserId) : IRequest<ErrorOr<Success>>;
