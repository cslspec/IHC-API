namespace Ihc.WebApi.Model;

/// <summary>
/// Represents the result of an attempt to connect to a time server.
/// This model provides details about the success or failure of the connection,
/// including optional time data.
/// </summary>
public class TimeServerConnectionResult
{
    /// <summary>
    /// Indicates whether the connection to the time server was successful.
    /// Returns <c>true</c> if the connection was successful; otherwise, <c>false</c>.
    /// </summary>
    public required bool ConnectionWasSuccessful { get; set; }

    /// <summary>
    /// Indicates whether the connection failed due to an unknown host.
    /// Returns <c>true</c> if the host could not be resolved; otherwise, <c>false</c>.
    /// </summary>
    public required bool ConnectionFailedDueToUnknownHost { get; set; }

    /// <summary>
    /// Indicates whether the connection failed due to errors other than an unknown host.
    /// Returns <c>true</c> if the connection failed for other reasons, such as a network error; otherwise, <c>false</c>.
    /// </summary>
    public required bool ConnectionFailedDueToOtherErrors { get; set; }

    /// <summary>
    /// Gets the number of seconds reported by the time server, if available.
    /// This value is <c>null</c> if the connection was unsuccessful or the time data is not provided.
    /// </summary>
    /// <example>1682345678</example>
    public required long? Seconds { get; set; }

    /// <summary>
    /// Gets the current time reported by the time server, if available.
    /// This value is <c>null</c> if the connection was unsuccessful or the time data is not provided.
    /// </summary>
    /// <example>2024-06-08T12:34:56Z</example>
    public required DateTime? Time { get; set; }
}
