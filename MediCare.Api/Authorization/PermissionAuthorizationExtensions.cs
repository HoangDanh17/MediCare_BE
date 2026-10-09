using MediCare.Domain.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace MediCare.Api.Authorization;

public static class PermissionAuthorizationExtensions
{
    public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Mỗi mã quyền trong catalog là một policy cùng tên
            foreach (var code in PermissionCatalog.Codes)
            {
                options.AddPolicy(code, policy => policy
                    .RequireAuthenticatedUser()
                    .AddRequirements(new PermissionRequirement(code)));
            }
        });

        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        return services;
    }
}