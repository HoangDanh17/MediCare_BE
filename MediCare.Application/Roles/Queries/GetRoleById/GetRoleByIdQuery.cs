using MediCare.Application.Common.Messaging;
using MediCare.Contract.Role;

namespace MediCare.Application.Roles.Queries.GetRoleById;

public sealed record GetRoleByIdQuery(Guid Id) : IQuery<RoleResponse>;