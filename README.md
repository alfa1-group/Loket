# Loket
Some projects to access the Loket REST API using C#.


## Loket.Api.Client
A Kiota generated C# client for Loket to access the REST API.

[![NuGet Badge](https://img.shields.io/nuget/v/Loket.Api.Client)](https://www.nuget.org/packages/Loket.Api.Client)


### Code Example
``` c#
var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services
            .AddSingleton(TimeProvider.System)
            .AddLogging()
            .AddLoketTokenStorageFileSystem(context.Configuration)
            .AddLoketKiotaAuthentication(context.Configuration);
    });

var host = builder.Build();

using var scope = host.Services.CreateScope();

var client = scope.ServiceProvider.GetRequiredService<LoketServiceClient>();

var getProvidersReponse = await client.Providers.GetAsProvidersGetResponseAsync(x => {
    x.QueryParameters.PageSize = 99;
    x.QueryParameters.OrderBy = OrderByBuilder<Provider>.OrderBy(p => p.Name).Build();
    x.QueryParameters.Filter = FilterBuilder<Provider>.Build(p => p.Name != "test");
});

var provider = getProvidersReponse?.Embedded?.FirstOrDefault();

Console.WriteLine(JsonSerializer.Serialize(provider));
```

---

## Authentication using Refresh and AccessToken

### Getting
For getting an AccessToken (based on RefreshToken), these two projects are used:

| Package | NuGet |
| :- | :- |
| Loket.Api.Client.Authentication | [![NuGet Badge](https://img.shields.io/nuget/v/Loket.Api.Client.Authentication)](https://www.nuget.org/packages/Loket.Api.Client.Authentication)
| Loket.Api.Client.Authentication.Kiota | [![NuGet Badge](https://img.shields.io/nuget/v/Loket.Api.Client.Authentication.Kiota)](https://www.nuget.org/packages/Loket.Api.Client.Authentication.Kiota)

Note that the `Loket.Api.Client.Authentication` can also be used when not using the Kiota generated client, but it is required for the `Loket.Api.Client.Authentication.Kiota` package.

### Storing
For storing and retrieving the RefreshToken and AccessToken, this project used:
- [Alfa1.TokenStorage](https://github.com/alfa1-group/Alfa1.TokenStorage)

---

## HowTo
In case the Loket REST interface is changed, you can regenerate the client using the following commands:

### Generate Loket.Api.Client
``` cmd
./kiota-generate.ps1
```