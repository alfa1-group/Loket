using System.ComponentModel.DataAnnotations;

namespace Loket.Api.Client.Authentication.Options;

public class LoketOptions
{
    [Required]
    [Url]
    public string BaseUrl { get; set; } = "https://api.loket.nl/v2";

    [Required]
    [Url]
    public string AuthenticationServerUrl { get; set; } = "https://oauth.loket.nl";

    /// <summary>
    /// Guid used by the application to uniquely identify itself to Loket.
    /// </summary>
    [Required]
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// Client secret (application password).
    /// </summary>
    [Required]
    public string ClientSecret { get; set; } = null!;
}