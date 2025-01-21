namespace Ihc.WebApi.Model;

/// <summary>
/// Configuration settings related to time synchronization and management.
/// </summary>
public class TimeSettings
{
    /// <summary>
    /// The name or network address of the time server. 
    /// The server should support the Time Protocol on port 37 (UDP).
    /// </summary>
    public required string? TimeServerName { get; set; }

    /// <summary>
    /// Indicates whether the system should synchronize its time with the specified time server.
    /// </summary>
    public required bool? Synchronize { get; set; }

    /// <summary>
    /// The interval, in hours, at which the system synchronizes its time with the time server.
    /// A value of <c>null</c> or <c>0</c> indicates that synchronization occurs only on demand.
    /// </summary>
    /// <example>12</example>
    public required int? SynchronizeInterval { get; set; }

    /// <summary>
    /// The offset from GMT/UTC, in hours, for the local time zone.
    /// Positive values indicate time zones ahead of GMT (e.g., +1 for Central European Time), 
    /// and negative values indicate time zones behind GMT (e.g., -5 for Eastern Standard Time).
    /// </summary>
    /// <example>-5</example>
    public required int? GmtOffset { get; set; }

    /// <summary>
    /// Specifies whether Daylight Saving Time (DST) is active.
    /// If enabled, the system adjusts the clock by adding one hour.
    /// </summary>
    /// <example>false</example>
    public required bool? UseDst { get; set; }

    /// <summary>
    /// The current system time, including the local time and its offset from UTC.
    /// </summary>
    /// <example>2024-12-14T12:34:56+01:00</example>
    public required DateTimeOffset? CurrentTime { get; set; }
}
