using System.Text.Json.Serialization;

namespace Ihc.WebApi.Model;

/// <summary>
/// The user groups available on the IHC controller.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<UserGroup>))]
public enum UserGroup
{
    /// <summary>
    /// Administrators with full access to the controller.
    /// </summary>
    Administrators,

    /// <summary>
    /// Ordinary users.
    /// </summary>
    Users
}
