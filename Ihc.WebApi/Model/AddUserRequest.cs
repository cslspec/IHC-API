namespace Ihc.WebApi.Model;

/// <summary>
/// Represents a new user account to create on the IHC controller.
/// </summary>
public class AddUserRequest
{
    /// <summary>
    /// The user's unique identifier.
    /// </summary>
    /// <example>jane</example>
    public required string Username { get; set; }

    /// <summary>
    /// The user's password.
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// The user's email address.
    /// </summary>
    /// <example>jane@example.com</example>
    public string? Email { get; set; }

    /// <summary>
    /// The user's first name.
    /// </summary>
    /// <example>Jane</example>
    public string? Firstname { get; set; }

    /// <summary>
    /// The user's last name.
    /// </summary>
    /// <example>Doe</example>
    public string? Lastname { get; set; }

    /// <summary>
    /// The user's phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// The user's group.
    /// </summary>
    /// <example>Users</example>
    public UserGroup Group { get; set; } = UserGroup.Users;
}
