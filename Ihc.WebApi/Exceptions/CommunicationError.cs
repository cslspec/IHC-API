namespace Ihc.WebApi.Exceptions;

/// <summary>
/// Standard IHC/HTTP/Communication error.
/// </summary>
public record CommunicationError(int Code, string Title, string Description);

/// <summary>
/// Standard IHC/HTTP/Communication error.
/// </summary>
public static class CommunicationErrors
{
    /// <summary>
    /// Login failed due to connection restructions error.
    /// </summary>
    public static readonly CommunicationError ConnectionRestriction =
        new(1006, "Connection restriction", "Login failed due to connection restructions error.");

    /// <summary>
    /// Login failed due to insufficient user rights error.
    /// </summary>
    public static readonly CommunicationError UserRights = 
        new(1007, "User rights", "Login failed due to insufficient user rights error.");

    /// <summary>
    /// Login failed due to account invalid error.
    /// </summary>
    public static readonly CommunicationError AccountInvalid =
        new(1008, "Account invalid", "Login failed due to account invalid error.");

    /// <summary>
    /// Unknown login error.
    /// </summary>
    public static readonly CommunicationError UnknownError =
        new(1009, "Unknown error", "Unknown login error.");

    public const int XML_FORMAT_ERROR = 1000;

    public const int XML_LOOKUP_ERROR = 1001;

    public const int XML_SERIALIZE_ERROR = 1002;

    public const int XML_DESERIALIZE_ERROR = 1003;

    public const int HTTP_CLIENT_SIDE_INTERNAL_ERROR = 1004;

    public const int HTTP_UNEXPECTED_CONTENT_ERROR = 1005;



    public const int FEATURE_NOT_IMPLEMENTED = 1010;

    public const int WEB_EXCEPTION_ERROR_BASE = 10000;
}
