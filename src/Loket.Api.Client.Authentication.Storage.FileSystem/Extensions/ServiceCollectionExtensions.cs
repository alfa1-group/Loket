using Loket.Api.Client.Authentication.Abstractions;
using Loket.Api.Client.Authentication.Storage.FileSystem;
using Loket.Api.Client.Authentication.Storage.FileSystem.Options;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoketTokenStorageFileSystem(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<LoketFileSystemOptions>()
            .Bind(configuration.GetSection(nameof(LoketFileSystemOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddMemoryCache();

        return services.AddSingleton<ILoketTokenStorageService, LoketTokenServiceFileSystem>();
    }
}