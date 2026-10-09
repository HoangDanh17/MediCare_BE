namespace MediCare.Persistence.Security;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Issuer { get; init; } = "Medicare";
    public string Audience { get; init; } = "Medicare.Client";
    public string Secret { get; init; } = null!;
    public int ExpiryMinutes { get; init; } = 60;
    public int RefreshTokenExpiryDays { get; init; } = 7;
}
