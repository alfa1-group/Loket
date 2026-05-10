using System.ComponentModel.DataAnnotations;

namespace Loket.Api.Client.Authentication.Storage.FileSystem.Options;

public class LoketFileSystemOptions
{
    [Required]
    public string RefreshTokenFilePath { get; set; } = null!;

    [Required]
    public string AccessTokenFilePath { get; set; } = null!;
}