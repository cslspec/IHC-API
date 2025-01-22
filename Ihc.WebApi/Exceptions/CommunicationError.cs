namespace Ihc.WebApi.Exceptions;

/// <summary>
/// Standard IHC/HTTP/Communication error.
/// </summary>
public record CommunicationError(int Code, string Title, string Description);

/// <summary>
/// Collection of standard IHC/HTTP/Communication errors.
/// </summary>
public static class CommunicationErrors
{
    /// <summary>
    /// Error occurred during XML serialization.
    /// </summary>
    public static readonly CommunicationError XmlSerializeError =
        new(1002, "XML serialize error", "An error occurred during XML serialization.");

    /// <summary>
    /// Error occurred during XML deserialization.
    /// </summary>
    public static readonly CommunicationError XmlDeserializeError =
        new(1003, "XML deserialize error", "An error occurred during XML deserialization.");

    /// <summary>
    /// Login failed due to connection restrictions.
    /// </summary>
    public static readonly CommunicationError ConnectionRestriction =
        new(1006, "Connection restriction", "Login failed due to connection restrictions.");

    /// <summary>
    /// Login failed due to insufficient user rights.
    /// </summary>
    public static readonly CommunicationError UserRights =
        new(1007, "User rights", "Login failed due to insufficient user rights.");

    /// <summary>
    /// Login failed due to an invalid account.
    /// </summary>
    public static readonly CommunicationError AccountInvalid =
        new(1008, "Account invalid", "Login failed due to an invalid account.");

    /// <summary>
    /// An unknown error occurred during login.
    /// </summary>
    public static readonly CommunicationError UnknownError =
        new(1009, "Unknown error", "An unknown error occurred during login.");
}
