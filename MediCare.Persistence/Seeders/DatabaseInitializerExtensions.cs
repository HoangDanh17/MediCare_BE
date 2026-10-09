using MediCare.Application.Common.Interfaces;
using MediCare.Domain.Common;
using MediCare.Domain.Repositories;
using MediCare.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MediCare.Persistence.Seeders;

public static class DatabaseInitializerExtensions
{
    public static async Task MigrateAndSeedAsync(this IHost host, CancellationToken cancellationToken = default)
    {
        await using var scope = host.Services.CreateAsyncScope();

        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger(typeof(DatabaseInitializerExtensions).FullName!);

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Database.MigrateAsync(cancellationToken);
        logger.LogInformation("Đã áp dụng migration cho database.");

        await scope.ServiceProvider
            .GetRequiredService<RoleSeeder>()
            .SeedAsync(cancellationToken);

        await SeedAdminUserAsync(host.Services, cancellationToken);

    }

    internal static async Task SeedAdminUserAsync(IServiceProvider services, CancellationToken ct = default)
    {
        await using var scope = services.CreateAsyncScope();
        var sp = scope.ServiceProvider;

        var config = sp.GetRequiredService<IConfiguration>();
        var email = config["SeedAdmin:Email"];
        var password = config["SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return;

        var users = sp.GetRequiredService<IUserRepository>();
        var roles = sp.GetRequiredService<IRoleRepository>();
        var hasher = sp.GetRequiredService<IPasswordHasher>();
        var unitOfWork = sp.GetRequiredService<IUnitOfWork>();

        if (await users.GetByEmailAsync(email, ct) is not null)
            return;

        var adminRole = await roles.GetByNameAsync("Admin", ct);   // dùng đúng tên role Admin của bạn
        if (adminRole is null)
            return;

        var created = User.Create(
            adminRole.Id, "Quản trị viên", email, hasher.HashPassword(password),
            "0000000000", "N/A", default, new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc));

        if (created.IsError)
            throw new InvalidOperationException($"Không tạo được admin: {created.FirstError.Description}");

        // Seed chạy trước khi CAP khởi động (app.Run) nên không publish domain event được.
        ((IAggregateRoot)created.Value).ClearDomainEvents();

        await users.AddAsync(created.Value, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }
}