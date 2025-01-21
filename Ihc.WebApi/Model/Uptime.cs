namespace Ihc.WebApi.Model;

/// <summary>
/// Represents the system's uptime information as a Data Transfer Object (DTO).
/// Provides detailed breakdowns of the total duration the system has been running.
/// </summary>
public class Uptime
{
    /// <summary>
    /// Gets the total uptime as a single <see cref="TimeSpan"/> value.
    /// Combines days, hours, minutes, seconds, and milliseconds into a unified representation.
    /// </summary>
    /// <example>1.13:30:45.123</example>
    public required TimeSpan Time { get; set; }

    /// <summary>
    /// Gets the total number of full days the system has been running.
    /// </summary>
    /// <example>1</example>
    public required int Days { get; set; }

    /// <summary>
    /// Gets the number of hours the system has been running, excluding full days.
    /// Range: 0–23.
    /// </summary>
    /// <example>13</example>
    public required int Hours { get; set; }

    /// <summary>
    /// Gets the number of minutes the system has been running, excluding full hours.
    /// Range: 0–59.
    /// </summary>
    /// <example>30</example>
    public required int Minutes { get; set; }

    /// <summary>
    /// Gets the number of seconds the system has been running, excluding full minutes.
    /// Range: 0–59.
    /// </summary>
    /// <example>45</example>
    public required int Seconds { get; set; }

    /// <summary>
    /// Gets the number of milliseconds the system has been running, excluding full seconds.
    /// Range: 0–999.
    /// </summary>
    /// <example>123</example>
    public required int Milliseconds { get; set; }

    /// <summary>
    /// Gets the total number of milliseconds the system has been running.
    /// Represents the cumulative uptime duration in milliseconds.
    /// </summary>
    /// <example>123456789</example>
    public required long TotalMilliseconds { get; set; }
}
