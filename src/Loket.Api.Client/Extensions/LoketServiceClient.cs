using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

// ReSharper disable once CheckNamespace
namespace Loket.Api.Client;

public partial class LoketServiceClient
{
    private const string DefaultBaseUrl = "https://api.loket.nl/v2";

    public LoketServiceClient(IAuthenticationProvider authenticationProvider, string baseUrl = DefaultBaseUrl)
        : this(CreateHttpClientRequestAdapter(authenticationProvider))
    {
        RequestAdapter.BaseUrl = baseUrl;
    }

    private static HttpClientRequestAdapter CreateHttpClientRequestAdapter(IAuthenticationProvider authenticationProvider)
    {
        return new HttpClientRequestAdapter(authenticationProvider)
        {
            BaseUrl = DefaultBaseUrl
        };
    }
}