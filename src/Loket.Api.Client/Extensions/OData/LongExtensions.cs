using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Loket.Api.Client.Extensions;

public static class LongExtensions
{
    /// <summary>
    /// If the value exceeds int.MaxValue, append "L" to indicate it's a long.
    /// </summary>
    public static string ToODataFormat(this long value)
    {
        var str = value.ToString(CultureInfo.InvariantCulture);
        return value > int.MaxValue ? $"{str}L" : str;
    }
}