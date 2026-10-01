namespace Ihc.WebApi.Model;

/// <summary>
/// Provides the connection and login settings for the IHC controller.
/// </summary>
public interface IControllerConfiguration
{
    /// <summary>
    /// The base address of the IHC controller.
    /// </summary>
    string Address { get; set; }

    /// <summary>
    /// The username used to authenticate with the controller.
    /// </summary>
    string UserName { get; set; }

    /// <summary>
    /// The password used to authenticate with the controller.
    /// </summary>
    string Password { get; set; }
}

/// <summary>
/// Stores the IHC controller connection settings loaded from application configuration.
/// </summary>
public class ControllerConfiguration : IControllerConfiguration
{
    /// <summary>
    /// The configuration section name containing controller settings.
    /// </summary>
    public const string Name = "controller";

    /// <inheritdoc />
    public required string Address { get; set; }

    /// <inheritdoc />
    public required string UserName { get; set; }

    /// <inheritdoc />
    public required string Password { get; set; }
}
