// ReSharper disable once CheckNamespace
namespace Loket.Api.Client.Extensions;

public static class DateTimeExtensions
{
    public static string ToODataFormat(this DateTime value)
    {
        return $"datetime'{value:yyyy-MM-ddTHH:mm:ss}'";
    }

    public static string ToODataFormat(this DateTimeOffset value)
    {
        return $"datetime'{value:yyyy-MM-ddTHH:mm:ss}'";
    }
}