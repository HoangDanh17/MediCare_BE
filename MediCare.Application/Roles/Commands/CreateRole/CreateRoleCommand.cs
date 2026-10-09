using MediCare.Application.Common.Messaging;
using MediCare.Contract.Role;
using MediCare.Domain.Permissions;

namespace MediCare.Application.Roles.Commands.CreateRole;

public record class CreateRoleCommand(string Name, List<PermissionId> PermissionIds) : ICommand<RoleResponse>;
