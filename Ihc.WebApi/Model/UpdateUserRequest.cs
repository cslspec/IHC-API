namespace Ihc.WebApi.Model;

/// <summary>
/// Represents changes to an existing user account on the IHC controller.
/// Properties that are left out keep their current value.
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// The user's new password.
    /// </summary>
    public string? Password { get; set; }

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
    public UserGroup? Group { get; set; }
}
