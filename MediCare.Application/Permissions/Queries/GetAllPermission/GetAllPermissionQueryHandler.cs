using ErrorOr;
using MediatR;
using MediCare.Contract.Role;
using MediCare.Domain.Repositories;

namespace MediCare.Application.Permissions.Queries.GetAllPermission;

public sealed class GetAllPermissionQueryHandler(
    IPermissionRepository permissionRepository) : IRequestHandler<GetAllPermissionQuery, ErrorOr<List<PermissionResponse>>>
{
    public async Task<ErrorOr<List<PermissionResponse>>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
    {
        var permissions = await permissionRepository.GetAllAsync(cancellationToken);
        return permissions.Select(p => new PermissionResponse(p.Id.Value, p.Name)).ToList();
    }
}
