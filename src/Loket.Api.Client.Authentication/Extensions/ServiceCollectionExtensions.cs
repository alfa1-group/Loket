using Loket.Api.Client.Authentication.Abstractions;
using Loket.Api.Client.Authentication.Implementations;
using Loket.Api.Client.Authentication.Interfaces;
using Loket.Api.Client.Authentication.Options;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up Loket services in an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoketAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<LoketOptions>()
            .Bind(configuration.GetSection(nameof(LoketOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddHttpClient();
        services.AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ILoketTokenClient, LoketTokenClient>();
        services.AddSingleton<ILoketTokenService, LoketTokenService>();

        if (services.All(s => s.ServiceType != typeof(ILoketTokenStorageService)))
        {
            throw new InvalidOperationException($"An implementation for {nameof(ILoketTokenStorageService)} is required. Please register it in the service collection.");
        }

        return services;
    }
}