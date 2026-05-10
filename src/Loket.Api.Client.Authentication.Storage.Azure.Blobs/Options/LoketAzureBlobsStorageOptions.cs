using System.ComponentModel.DataAnnotations;

namespace Loket.Api.Client.Authentication.Storage.Azure.Blobs.Options;

public class LoketAzureBlobsStorageOptions
{
    [Required]
    public string ConnectionString { get; set; } = null!;

    [Required]
    public string ContainerName { get; set; } = null!;

    [Required] 
    public string RefreshTokenFilePath { get; set; } = "refreshtoken.txt";

    [Required]
    public string AccessTokenFilePath { get; set; } = "accesstoken.txt";
}