namespace Ihc.WebApi.Model;

/// <summary>
/// Describes an authenticated IHC session and its associated user.
/// </summary>
public interface IAuthToken
{
    /// <summary>Gets the unique identifier for this session.</summary>
    Guid Id { get; init; }

    /// <summary>Gets the time at which the session was created.</summary>
    DateTime LoginTime { get; init; }

    /// <summary>Gets or sets the time at which the session was closed, if logged out.</summary>
    DateTime? LogoutTime { get; set; }

    /// <summary>Gets the time at which the session expires.</summary>
    DateTime ExpireTime { get; init; }

    /// <summary>Gets or sets the controller's authentication token.</summary>
    string? Token { get; set; }

    /// <summary>Gets the user associated with this session.</summary>
    IhcUser User { get; init; }
}

/// <summary>
/// Stores the authentication token and lifecycle information for an IHC session.
/// </summary>
public class AuthToken : IAuthToken
{
    /// <inheritdoc />
    public required Guid Id { get; init; }

    /// <inheritdoc />
    public required DateTime LoginTime { get; init; }

    /// <inheritdoc />
    public DateTime? LogoutTime { get; set; }

    /// <inheritdoc />
    public required DateTime ExpireTime { get; init; }

    /// <inheritdoc />
    public string? Token { get; set; }

    /// <inheritdoc />
    public required IhcUser User { get; init; }
}
