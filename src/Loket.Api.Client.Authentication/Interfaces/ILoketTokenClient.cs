using Duende.IdentityModel.Client;

namespace Loket.Api.Client.Authentication.Interfaces;

public interface ILoketTokenClient
{
    /// <summary>
    /// Request a new access token + refresh token using the provided refresh token. 
    /// The response contains a new access token and a new refresh token.
    /// </summary>
    Task<TokenResponse> RequestTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}