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
    x.QueryParameters.Filter = FilterBuilder<Provider>.Build(p => p.Name != "test");
});

var provider = getProvidersReponse?.Embedded?.FirstOrDefault();

Console.WriteLine(JsonSerializer.Serialize(provider));
```


## Loket.Api.Client.Authentication
Implementation of the OAuth authentication for Loket.
It uses the `Loket.Api.Client.Authentication.Abstractions` interfaces package to store the Refresh Token in a storage.

[![NuGet Badge](https://img.shields.io/nuget/v/Loket.Api.Client.Authentication)](https://www.nuget.org/packages/Loket.Api.Client.Authentication)


## Loket.Api.Client.Authentication.Kiota
Contains an implementation of the `IAuthenticationProvider` interface for Kiota, which is used to authenticate requests to the Loket API.

[![NuGet Badge](https://img.shields.io/nuget/v/Loket.Api.Client.Authentication.Kiota)](https://www.nuget.org/packages/Loket.Api.Client.Authentication.Kiota)


## Loket.Api.Client.Authentication.Abstractions
An interface `ILoketTokenStorageService` which defines how to store and retrieve the Refresh and Access Tokens.

This interface is implemented by several packages, like:

| Package | NuGet |
| :- | :- |
| Loket.Api.Client.Authentication.Storage.Azure.Blobs | [![NuGet Badge](https://img.shields.io/nuget/v/Loket.Api.Client.Authentication.Storage.Azure.Blobs)](https://www.nuget.org/packages/Loket.Api.Client.Authentication.Storage.Azure.Blobs)
| Loket.Api.Client.Authentication.Storage.FileSystem | [![NuGet Badge](https://img.shields.io/nuget/v/Loket.Api.Client.Authentication.Storage.FileSystem)](https://www.nuget.org/packages/Loket.Api.Client.Authentication.Storage.FileSystem)
| Loket.Api.Client.Authentication.Storage.SqlServer | [![NuGet Badge](https://img.shields.io/nuget/v/Loket.Api.Client.Authentication.Storage.SqlServer)](https://www.nuget.org/packages/Loket.Api.Client.Authentication.Storage.SqlServer)
| Loket.Api.Client.Authentication.Storage.PostgreSQL | [![NuGet Badge](https://img.shields.io/nuget/v/Loket.Api.Client.Authentication.Storage.PostgreSQL)](https://www.nuget.org/packages/Loket.Api.Client.Authentication.Storage.PostgreSQL)


---


## HowTo
In case the Loket REST interface is changed, you can regenerate the client using the following commands:

### Generate Loket.Api.Client
``` cmd
./kiota-generate.ps1
```