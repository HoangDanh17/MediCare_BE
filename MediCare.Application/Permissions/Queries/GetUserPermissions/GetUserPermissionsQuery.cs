using MediCare.Application.Common.Messaging;
using MediCare.Contract.User;

namespace MediCare.Application.Permissions.Queries.GetUserPermissions;

public sealed record GetUserPermissionsQuery(Guid UserId)
: IQuery<List<UserPermissionStateResponse>>;
