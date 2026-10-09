using MediCare.Domain.Common;

namespace MediCare.Domain.Permissions;

public sealed class Permission : AggregateRoot<PermissionId>
{
    public string Name { get; private set; } = null!;
    private Permission() { }
}
