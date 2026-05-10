using Duende.IdentityModel.Client;
using Loket.Api.Client.Authentication.Interfaces;
using Loket.Api.Client.Authentication.Options;
using Microsoft.Extensions.Options;

namespace Loket.Api.Client.Authentication.Implementations;

internal class LoketTokenClient(IHttpClientFactory httpClientFactory, IOptions<LoketOptions> LoketOptions) : ILoketTokenClient
{
    private readonly TokenClientOptions _tokenClientOptions = new()
    {
        Address = $"{LoketOptions.Value.AuthenticationServerUrl}/token",
        ClientId = LoketOptions.Value.ClientId,
        ClientSecret = LoketOptions.Value.ClientSecret
    };

    public async Task<TokenResponse> RequestTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var tokenHttpClient = httpClientFactory.CreateClient();
        var client = new TokenClient(tokenHttpClient, _tokenClientOptions);

        return await client.RequestRefreshTokenAsync(refreshToken, cancellationToken: cancellationToken);
    }
}