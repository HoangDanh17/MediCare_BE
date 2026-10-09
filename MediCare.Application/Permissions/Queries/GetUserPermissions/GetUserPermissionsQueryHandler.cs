using ErrorOr;
using MediatR;
using MediCare.Application.Common.Interfaces;
using MediCare.Contract.User;
using MediCare.Domain.Users;

namespace MediCare.Application.Permissions.Queries.GetUserPermissions;

public sealed class GetUserPermissionsQueryHandler(IUserPermissionReader reader)
 : IRequestHandler<GetUserPermissionsQuery, ErrorOr<List<UserPermissionStateResponse>>>
{
    public async Task<ErrorOr<List<UserPermissionStateResponse>>> Handle(
        GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        var states = await reader.GetStatesAsync(new UserId(request.UserId), cancellationToken);

        if (states is null)
            return UserErrors.NotFound;

        return states;
    }
}
