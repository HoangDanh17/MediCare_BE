using MediCare.Domain.Users;

namespace MediCare.Application.Common.Interfaces;

public sealed record AccessToken(string Value, DateTime ExpiresAtUtc);
public interface IJwtTokenGenerator
{
    AccessToken Generate(User user);
}
