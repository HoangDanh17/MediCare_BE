using ErrorOr;
using MediCare.Application.Common.Interfaces;
using MediCare.Domain.Users;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Security.Cryptography;
using System.Text;
namespace MediCare.Persistence.Security;

public sealed class RedisRefreshTokenStore(IConnectionMultiplexer redis, IOptions<JwtOptions> options)
    : IRefreshTokenStore
{
    private IDatabase Db => redis.GetDatabase();
    private TimeSpan Lifetime => TimeSpan.FromDays(options.Value.RefreshTokenExpiryDays);

    public async Task<ErrorOr<UserId>> ConsumeAsync(string token, CancellationToken cancellationToken = default)
    {
        var hash = Hash(token);

        // GETDEL: lấy và xóa trong một lệnh nguyên tử,
        // hai request cùng token không thể cùng thành công.
        var value = await Db.StringGetDeleteAsync(TokenKey(hash));

        if (value.IsNullOrEmpty)
        {
            // Token không còn, nhưng nếu có dấu vết "đã dùng" thì token cũ bị dùng lại.
            var usedBy = await Db.StringGetAsync(UsedKey(hash));
            if (!usedBy.IsNullOrEmpty && Guid.TryParse(usedBy.ToString(), out var id))
                await RevokeAllAsync(new UserId(id), cancellationToken);

            return UserErrors.InvalidRefreshToken;
        }

        var userId = new UserId(Guid.Parse(value.ToString()));

        await Db.SetRemoveAsync(UserKey(userId), hash);
        await Db.StringSetAsync(UsedKey(hash), userId.Value.ToString(), Lifetime);

        return userId;
    }

    public async Task<string> IssueAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var token = WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32));
        var hash = Hash(token);

        await Db.StringSetAsync(TokenKey(hash), userId.Value.ToString(), Lifetime);
        await Db.SetAddAsync(UserKey(userId), hash);
        await Db.KeyExpireAsync(UserKey(userId), Lifetime);

        return token;
    }

    public async Task RevokeAllAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var hashes = await Db.SetMembersAsync(UserKey(userId));

        var keys = hashes
            .Select(h => (RedisKey)TokenKey(h.ToString()))
            .Append(UserKey(userId))
            .ToArray();

        await Db.KeyDeleteAsync(keys);
    }

    public async Task RevokeAsync(string token, CancellationToken cancellationToken = default)
    {
        var hash = Hash(token);
        var value = await Db.StringGetDeleteAsync(TokenKey(hash));

        if (!value.IsNullOrEmpty && Guid.TryParse(value.ToString(), out var id))
            await Db.SetRemoveAsync(UserKey(new UserId(id)), hash);
    }

    private static string Hash(string token) =>
       Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

    private static string TokenKey(string hash) => $"rt:{hash}";
    private static string UsedKey(string hash) => $"rt:used:{hash}";
    private static string UserKey(UserId id) => $"rt:user:{id.Value}";
}
