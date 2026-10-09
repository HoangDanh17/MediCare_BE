using ErrorOr;
using MediCare.Domain.Common;
using MediCare.Domain.Permissions;
using MediCare.Domain.Roles.Events;

namespace MediCare.Domain.Roles;

public sealed class Role : AggregateRoot<RoleId>
{
    public string Name { get; private set; } = null!;
    private readonly List<Permission> _permissions = [];
    public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

    private Role() { }
    private Role(RoleId id, string name)
    {
        Id = id;
        Name = name;
    }
    public static ErrorOr<Role> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoleErrors.InvalidName;

        var roleId = RoleId.New();

        var role = new Role(roleId, name.Trim());

        role.RaiseDomainEvent(new RoleCreatedDomainEvent(roleId, name, DateTime.Now));
        return role;
    }

    public ErrorOr<Success> Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoleErrors.InvalidName;

        Name = name.Trim();

        RaiseDomainEvent(new RoleUpdatedDomainEvent(Id, Name, DateTime.Now));

        return Result.Success;
    }

    public void Delete() => RaiseDomainEvent(new RoleDeletedDomainEvent(Id, DateTime.Now));

    public ErrorOr<Success> AddPermission(Permission permission)
    {
        if (_permissions.Any(p => p.Id == permission.Id))
            return RoleErrors.PermissionAlreadyGranted;

        _permissions.Add(permission);

        RaiseDomainEvent(new RolePermissionGrantedDomainEvent(Id, permission.Id, DateTime.Now));

        return Result.Success;
    }

    public ErrorOr<Success> RemovePermission(Permission permission)
    {
        var existingPermission = _permissions.FirstOrDefault(p => p.Id == permission.Id);

        if (existingPermission is null)
            return RoleErrors.PermissionNotGranted;

        _permissions.Remove(existingPermission);

        RaiseDomainEvent(new RolePermissionRemovedDomainEvent(Id, permission.Id, DateTime.Now));

        return Result.Success;
    }
}
