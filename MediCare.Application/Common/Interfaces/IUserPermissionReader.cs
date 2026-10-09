using MediCare.Contract.User;
using MediCare.Domain.Users;

namespace MediCare.Application.Common.Interfaces;

public interface IUserPermissionReader
{
    /// <summary>Trạng thái từng quyền của user, hoặc null nếu user không tồn tại.</summary>
    Task<List<UserPermissionStateResponse>?> GetStatesAsync(UserId userId, CancellationToken cancellationToken = default);

    /// <summary>Mã các quyền user thực sự có (dùng cho authorization).</summary>
    Task<HashSet<string>> GetEffectivePermissionCodesAsync(UserId userId, CancellationToken cancellationToken = default);
}
