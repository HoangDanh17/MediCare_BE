using MediCare.Domain.Permissions;

namespace MediCare.Domain.Repositories;

public interface IPermissionRepository
{
    Task<List<Permission>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Permission?> GetByIdAsync(PermissionId id, CancellationToken cancellationToken = default);
    Task<List<Permission>> GetByIdsAsync(List<PermissionId> ids, CancellationToken cancellationToken);
}
