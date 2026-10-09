using MediCare.Application.Common.Messaging;
using MediCare.Contract.Role;

namespace MediCare.Application.Permissions.Queries.GetAllPermission;

public sealed record GetAllPermissionQuery : IQuery<List<PermissionResponse>>;