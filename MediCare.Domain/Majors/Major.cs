using ErrorOr;
using MediCare.Domain.Common;
using MediCare.Domain.Majors.Events;

namespace MediCare.Domain.Majors;

public sealed class Major : AggregateRoot<MajorId>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    private Major() { }
    private Major(MajorId id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public static ErrorOr<Major> Create(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return MajorErrors.InvalidName;

        var majorId = MajorId.New();

        var major = new Major(majorId, name, description);

        major.RaiseDomainEvent(new MajorCreatedDomainEvent(majorId, name, description, DateTime.Now));

        return major;
    }

    public ErrorOr<Major> Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return MajorErrors.InvalidName;

        Name = name;
        Description = description;

        var major = new Major(Id, name, description);

        RaiseDomainEvent(new MajorUpdatedDomainEvent(Id, name, description, DateTime.Now));

        return major;
    }

    public void Delete()
    {
        RaiseDomainEvent(new MajorDeletedDomainEvent(Id, DateTime.Now));
    }
}
