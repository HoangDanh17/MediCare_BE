using DomainMajor = MediCare.Domain.Majors.Major;

namespace MediCare.Contract.Major;

public sealed record MajorResponse(
    Guid Id,
    string Name,
    string? Description)
{
    public static MajorResponse From(DomainMajor major) =>
        new(major.Id.Value, major.Name, major.Description);
}
