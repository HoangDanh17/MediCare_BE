using ErrorOr;
using MediatR;
using MediCare.Contract.Auth;

namespace MediCare.Application.Auths.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<ErrorOr<TokenResponse>>;