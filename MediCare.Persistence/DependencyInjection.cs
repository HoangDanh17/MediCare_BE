using MediCare.Application.Common.Interfaces;
using MediCare.Domain.Repositories;
using MediCare.Persistence.Common;
using MediCare.Persistence.Messaging;
using MediCare.Persistence.Repositories;
using MediCare.Persistence.Security;
using MediCare.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Text;

namespace MediCare.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var postgres = configuration.GetConnectionString("Postgres")
             ?? throw new InvalidOperationException("Thiếu ConnectionStrings:Postgres.");

        var rabbit = configuration.GetSection("RabbitMQ");
        var rabbitHost = rabbit["HostName"] ?? throw new InvalidOperationException("Thiếu RabbitMQ:HostName.");
        var rabbitUser = rabbit["UserName"] ?? throw new InvalidOperationException("Thiếu RabbitMQ:UserName.");
        var rabbitPass = rabbit["Password"] ?? throw new InvalidOperationException("Thiếu RabbitMQ:Password.");
        var rabbitPort = rabbit.GetValue("Port", 5672);
        var exchange = rabbit["ExchangeName"] ?? "medicare.events";

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(postgres));

        services.AddCap(x =>
        {
            x.UseEntityFramework<AppDbContext>();   // outbox dùng chung AppDbContext
            x.UseRabbitMQ(o =>
            {
                o.HostName = rabbitHost;
                o.Port = rabbitPort;
                o.UserName = rabbitUser;
                o.Password = rabbitPass;
                o.ExchangeName = exchange;
            });

            x.DefaultGroupName = "medicare.api";
            x.FailedRetryCount = 5;
            x.FailedRetryInterval = 60;                 // giây
            x.SucceedMessageExpiredAfter = 24 * 3600;   // dọn message thành công sau 1 ngày

            if (configuration.GetValue<bool>("Cap:EnableDashboard"))
                x.UseDashboard();
        });

        var redis = configuration.GetConnectionString("Redis")
            ?? throw new InvalidOperationException("Thiếu ConnectionStrings:Redis.");

        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redis));

        services.AddScoped<RoleSeeder>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IMajorRepository, MajorRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();


        services.AddScoped<IUserPermissionReader, UserPermissionReader>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEventPublisher, CapEventPublisher>();


        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .Validate(o => !string.IsNullOrWhiteSpace(o.Secret) && Encoding.UTF8.GetByteCount(o.Secret) >= 32,
              "Jwt:Secret phải dài ít nhất 32 ký tự.")
            .ValidateOnStart();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IRefreshTokenStore, RedisRefreshTokenStore>();

        return services;

    }
}
