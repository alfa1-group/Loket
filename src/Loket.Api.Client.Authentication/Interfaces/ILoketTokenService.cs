namespace Loket.Api.Client.Authentication.Interfaces;

public interface ILoketTokenService
{
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);

    Task<string> RefreshTokenAsync(CancellationToken cancellationToken = default);
}