using Azure.Storage.Blobs;
using Loket.Api.Client.Authentication.Abstractions;
using Loket.Api.Client.Authentication.Storage.Azure.Blobs;
using Loket.Api.Client.Authentication.Storage.Azure.Blobs.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoketTokenStorageAzureBlobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<LoketAzureBlobsStorageOptions>()
            .Bind(configuration.GetSection(nameof(LoketAzureBlobsStorageOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddMemoryCache();

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<LoketAzureBlobsStorageOptions>>();
            return new BlobContainerClient(options.Value.ConnectionString, options.Value.ContainerName);
        });

        return services.AddSingleton<ILoketTokenStorageService, LoketTokenServiceAzureBlobs>();
    }
}