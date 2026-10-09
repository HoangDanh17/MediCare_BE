namespace MediCare.Domain.Majors;

public readonly record struct MajorId(Guid Value)
{
    public static MajorId New() => new(Guid.NewGuid());
}
