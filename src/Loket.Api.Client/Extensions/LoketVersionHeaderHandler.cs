using System.Net.Http;
using System.Net.Http.Headers;

namespace Loket.Api.Client;

internal sealed class LoketVersionHeaderHandler : DelegatingHandler
{
    private const string JsonMediaType = "application/json";

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content?.Headers.ContentType?.MediaType?.Equals(JsonMediaType, StringComparison.OrdinalIgnoreCase) == true)
        {
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(JsonMediaType);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
