namespace Ihc.WebApi.Model.Time;

/// <summary>
/// Represents changes to the time settings of the IHC controller.
/// Properties that are left out keep their current value.
/// </summary>
public class UpdateTimeSettingsRequest
{
    /// <summary>
    /// The name or network address of the time server.
    /// The server should support the Time Protocol on port 37 (UDP).
    /// </summary>
    /// <example>time.example.com</example>
    public string? TimeServerName { get; set; }

    /// <summary>
    /// Indicates whether the controller should synchronize its time with the time server.
    /// The time server is tested before synchronization is enabled.
    /// </summary>
    /// <example>true</example>
    public bool? Synchronize { get; set; }

    /// <summary>
    /// The interval, in hours, at which the controller synchronizes its time with the time server.
    /// </summary>
    /// <example>12</example>
    public int? SynchronizeInterval { get; set; }

    /// <summary>
    /// The offset from GMT/UTC, in hours, for the local time zone.
    /// </summary>
    /// <example>1</example>
    public int? GmtOffset { get; set; }

    /// <summary>
    /// Specifies whether Daylight Saving Time (DST) is used.
    /// </summary>
    /// <example>true</example>
    public bool? UseDst { get; set; }

    /// <summary>
    /// Sets the controller clock to this time. Only used when the controller does not synchronize with a time server.
    /// </summary>
    /// <example>2024-12-14T12:34:56+01:00</example>
    public DateTimeOffset? CurrentTime { get; set; }
}
