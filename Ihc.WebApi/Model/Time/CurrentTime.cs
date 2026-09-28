namespace Ihc.WebApi.Model.Time;

/// <summary>
/// Represents the current system time.
/// </summary>
public class CurrentTime
{
    /// <summary>
    /// The current system local time.
    /// </summary>
    /// <example>2024-12-14T12:34:56</example>
    public required DateTime CurrentLocalTime { get; set; }
}
