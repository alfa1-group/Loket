using Loket.Api.Client;
using Loket.Api.Client.Authentication.Kiota;
using Loket.Api.Client.Authentication.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Setup Dependency Injection for the <see cref="LoketServiceClient"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoketKiotaAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLoketAuthentication(configuration);
        services.AddSingleton<LoketAuthenticationProvider>();
        services.AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddSingleton(sp =>
        {
            var authenticationProvider = sp.GetRequiredService<LoketAuthenticationProvider>();
            var options = sp.GetRequiredService<IOptions<LoketOptions>>();
            
            return new LoketServiceClient(authenticationProvider, options.Value.BaseUrl);
        });
    }
}