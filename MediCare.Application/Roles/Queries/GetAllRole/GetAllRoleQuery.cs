using MediCare.Application.Common.Messaging;
using MediCare.Contract.Role;

namespace MediCare.Application.Roles.Queries.GetAllRole;

public sealed record GetAllRoleQuery() : IQuery<IReadOnlyList<RoleResponse>>;
