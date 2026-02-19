namespace Ihc.WebApi.Model;

/// <summary>
/// Represents a user account with authentication and profile information for the IHC system.
/// </summary>
/// <remarks>The IhcUser class encapsulates user credentials, contact details, group membership, and audit
/// information such as creation and last login dates. Instances of this class are typically used to manage user
/// authentication, authorization, and profile data within the application. Sensitive properties such as Password and
/// AuthToken should be handled securely and not exposed in logs or user interfaces.</remarks>
public class IhcUser
{
    /// <summary>
    /// The user's unique identifier.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// The user's password.
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// The user's email address.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// The user's first name.
    /// </summary>
    public required string Firstname { get; init; }

    /// <summary>
    /// The user's last name.
    /// </summary>
    public required string Lastname { get; init; }

    /// <summary>
    /// The user's phone number.
    /// </summary>
    public required string Phone { get; init; }

    /// <summary>
    /// The user's group.
    /// </summary>
    public required string Group { get; init; }

    /// <summary>
    /// IHC project name.
    /// </summary>
    public required string Project { get; init; }

    /// <summary>
    /// The date the user was created.
    /// </summary>
    public required DateTimeOffset CreatedDate { get; init; }

    /// <summary>
    /// The date the user last logged in.
    /// </summary>
    public required DateTimeOffset LoginDate { get; init; }

    /// <summary>
    /// Internal authentication token for the user.
    /// </summary>
    public string? AuthToken { get; set; }
}
