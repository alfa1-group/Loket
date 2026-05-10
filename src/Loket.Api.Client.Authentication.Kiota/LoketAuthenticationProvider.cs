using Loket.Api.Client.Authentication.Interfaces;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace Loket.Api.Client.Authentication.Kiota;

internal class LoketAuthenticationProvider(ILoketTokenService tokenService) : IAuthenticationProvider
{
    private const string AuthorizationHeaderKey = "Authorization";

    public async Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
    {
        var accessToken = await tokenService.GetAccessTokenAsync(cancellationToken);

        request.Headers.Add(AuthorizationHeaderKey, $"Bearer {accessToken}");
    }
}