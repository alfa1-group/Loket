namespace Loket.Api.Client.Builders.Filter;

public class TimestampFilter
{
    /// <summary>
    /// Timestamp of the last synchronization.
    /// The value has no relation with actual date or time. As such it cannot be converted to a date or time value. The timestamp is a rowversion value.
    /// It's defined as a 64-bit integer that is incremented each time a record is updated.
    /// </summary>
    public long Timestamp { get; set; }
}