using Loket.Api.Client.Authentication.Abstractions;
using Loket.Api.Client.Authentication.Storage.SqlServer;
using Loket.Api.Client.Authentication.Storage.SqlServer.Data;
using Loket.Api.Client.Authentication.Storage.SqlServer.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoketTokenStorageSqlServer(this IServiceCollection services, IConfiguration configuration, ServiceLifetime dbContextLifetime = ServiceLifetime.Scoped)
    {
        services.AddOptions<LoketEntityFrameworkCoreStorageOptions>()
            .Bind(configuration.GetSection("LoketSqlServerStorageOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddMemoryCache();

        services.AddDbContext<LoketTokenDbContext>((serviceProvider, options) =>
        {
            var connectionString = configuration.GetConnectionString(serviceProvider.GetOptions().ConnectionStringName);

            options.UseSqlServer(connectionString);
        }, dbContextLifetime);

        services.EnsureLoketTokenTableExists();

        return dbContextLifetime switch
        {
            ServiceLifetime.Singleton => services.AddSingleton<ILoketTokenStorageService, LoketTokenStorageEntityFrameworkCoreService>(),
            ServiceLifetime.Scoped => services.AddScoped<ILoketTokenStorageService, LoketTokenStorageEntityFrameworkCoreService>(),
            _ => services.AddTransient<ILoketTokenStorageService, LoketTokenStorageEntityFrameworkCoreService>()
        };
    }

    private static LoketEntityFrameworkCoreStorageOptions GetOptions(this IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<IOptions<LoketEntityFrameworkCoreStorageOptions>>().Value;
    }

    private static void EnsureLoketTokenTableExists(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<LoketTokenDbContext>();

        dbContext.Database.EnsureCreated();
        dbContext.EnsureLoketTokenTableExists();
    }
}