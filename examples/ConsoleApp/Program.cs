using System.Text.Json;
using Loket.Api.Client;
using Loket.Api.Client.Builders.Filter;
using Loket.Api.Client.Builders.OrderBy;
using Loket.Api.Client.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services
            .AddSingleton(TimeProvider.System)
            .AddLogging()
            .AddLoketTokenStorageAzureBlobs(context.Configuration)
            .AddLoketKiotaAuthentication(context.Configuration);
    });

var host = builder.Build();

using var scope = host.Services.CreateScope();

var client = scope.ServiceProvider.GetRequiredService<LoketServiceClient>();

var getProvidersReponse = await client.Providers.GetAsProvidersGetResponseAsync(x => {
    x.QueryParameters.PageSize = 99;
    x.QueryParameters.OrderBy = OrderByBuilder<Provider>.OrderByDescending(p => p.Name).Build();
    x.QueryParameters.Filter = FilterBuilder<Provider>.Build(p => p.Name != "test");
});

var provider = getProvidersReponse?.Embedded?.FirstOrDefault();

Console.WriteLine(JsonSerializer.Serialize(provider));