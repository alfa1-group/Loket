using System.Text.Json;
using Duende.IdentityModel.Client;
using Loket.Api.Client.Authentication.Abstractions;
using Loket.Api.Client.Authentication.Interfaces;
using Microsoft.Extensions.Logging;

namespace Loket.Api.Client.Authentication.Implementations;

internal class LoketTokenService(
    ILogger<LoketTokenService> logger,
    ILoketTokenStorageService tokenStorageService,
    ILoketTokenClient LoketTokenClient) : ILoketTokenService
{
    // Ensure that only one thread refreshes the tokens at a time
    private static readonly SemaphoreSlim RefreshTokenSemaphore = new(1, 1);

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        // First we check if we have a token in storage.
        var accessToken = await tokenStorageService.RetrieveAccessTokenAsync(cancellationToken);
        if (!string.IsNullOrEmpty(accessToken))
        {
            return accessToken;
        }

        // If expired or not present, refresh the AccessToken by contacting the authentication server using the RefreshToken from storage.
        return await RefreshTokenAsync(cancellationToken);
    }

    public async Task<string> RefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        await RefreshTokenSemaphore.WaitAsync(cancellationToken);

        try
        {
            // The client will issue the refresh request and should get a new refresh token + access token in response
            var (currentRefreshToken, response) = await RequestRefreshTokenAsync(cancellationToken);

            // Store the new refresh token back in storage as the previous one is now invalid.
            try
            {
                await tokenStorageService.StoreRefreshTokenAsync(currentRefreshToken, response.RefreshToken!, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Storing the new RefreshToken failed. Here is the token: {response.RefreshToken}", ex);
            }

            if (string.IsNullOrWhiteSpace(response.AccessToken))
            {
                logger.LogError("The access token is null or empty. ({ErrorType} {Error} {ErrorDescription}).", response.ErrorType, response.Error, response.ErrorDescription);
            }

            // Store the access token for reuse
            var currentAccessToken = await tokenStorageService.RetrieveAccessTokenAsync(cancellationToken);
            return await tokenStorageService.StoreAccessTokenAsync(currentAccessToken, response.AccessToken!, TimeSpan.FromSeconds(response.ExpiresIn), cancellationToken);
        }
        finally
        {
            RefreshTokenSemaphore.Release();
        }
    }

    private async Task<(string CurrentRefreshToken, TokenResponse TokenResponse)> RequestRefreshTokenAsync(CancellationToken cancellationToken)
    {
        // For refreshing the token we first need to fetch the current refresh token from storage, as we need to provide it in the request to the authentication server.
        // This is required to get a new access token and refresh token.
        var currentRefreshToken = await tokenStorageService.RetrieveRefreshTokenAsync(cancellationToken);

        // Now we can request a new access token using the refresh token
        var response = await LoketTokenClient.RequestTokenAsync(currentRefreshToken, cancellationToken);

        if (string.IsNullOrWhiteSpace(response.RefreshToken))
        {
            logger.LogError("There was a problem fetching a new auth token from Loket. ({ErrorType} {Error} {ErrorDescription}).", response.ErrorType, response.Error, response.ErrorDescription);
            logger.LogDebug("There was a problem fetching a new auth token from Loket. {TokenResponse}", JsonSerializer.Serialize(response));

            throw new Exception("Loket did not return a new auth token.", response.Exception);
        }

        return (currentRefreshToken, response);
    }
}