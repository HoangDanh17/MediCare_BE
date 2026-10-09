using ErrorOr;
using MediCare.Domain.Users;

namespace MediCare.Application.Common.Interfaces;

public interface IRefreshTokenStore
{
    /// <summary>Tạo refresh token mới, trả về token gốc (chỉ trả đúng một lần).</summary>
    Task<string> IssueAsync(UserId userId, CancellationToken cancellationToken = default);

    /// <summary>Dùng một lần: xóa token và trả về chủ sở hữu, hoặc lỗi nếu không hợp lệ.</summary>
    Task<ErrorOr<UserId>> ConsumeAsync(string token, CancellationToken cancellationToken = default);

    Task RevokeAsync(string token, CancellationToken cancellationToken = default);

    Task RevokeAllAsync(UserId userId, CancellationToken cancellationToken = default);
}
