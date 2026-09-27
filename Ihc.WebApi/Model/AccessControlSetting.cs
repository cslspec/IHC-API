namespace Ihc.WebApi.Model;

/// <summary>
/// Describes the access permissions for an IHC feature.
/// </summary>
public class AccessControlSetting
{
    /// <summary>The identifier of this access-control setting.</summary>
    public required int Id { get; set; }

    /// <summary>The name of the feature or access-control setting.</summary>
    public required string Name { get; set; }

    /// <summary>A human-readable description of the feature.</summary>
    public required string Description { get; set; }

    /// <summary>Whether access through USB is permitted.</summary>
    public required bool Usb { get; set; }

    /// <summary>Whether access from the internal network is permitted.</summary>
    public required bool Internal { get; set; }

    /// <summary>Whether access from the external network is permitted.</summary>
    public required bool External { get; set; }
}
