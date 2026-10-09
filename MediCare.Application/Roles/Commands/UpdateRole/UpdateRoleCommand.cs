using MediCare.Application.Common.Messaging;
using MediCare.Contract.Role;
using MediCare.Domain.Permissions;
using MediCare.Domain.Roles;

namespace MediCare.Application.Roles.Commands.UpdateRole;

public record class UpdateRoleCommand(RoleId Id, string Name, List<PermissionId> PermissionIds) : ICommand<RoleResponse>;
