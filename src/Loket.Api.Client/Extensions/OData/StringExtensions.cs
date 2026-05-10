// ReSharper disable once CheckNamespace
namespace Loket.Api.Client.Extensions;

public static class StringExtensions
{
    internal const string ODataNullLiteral = "null";

    /// <summary>
    /// Escapes a string value so it can be safely used in an OData $filter expression.
    /// Handles OData single-quote escaping and URL-encodes reserved characters.
    /// </summary>
    public static string ToODataFormat(this string? value)
    {
        // If null, return "null" (OData literal for null)
        if (value == null)
        {
            return ODataNullLiteral;
        }

        // OData-specific: double the single quotes
        var escaped = value.Replace("'", "''");

        // Wrap in single quotes for OData literal
        return $"'{escaped}'";
    }
}