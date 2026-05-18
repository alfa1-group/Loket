using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Net.Http;

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
        var httpClient = KiotaClientFactory.Create(new LoketVersionHeaderHandler
        {
            InnerHandler = new HttpClientHandler()
        });

        return new HttpClientRequestAdapter(authenticationProvider, null, null, httpClient, null)
        {
            BaseUrl = DefaultBaseUrl
        };
    }
}