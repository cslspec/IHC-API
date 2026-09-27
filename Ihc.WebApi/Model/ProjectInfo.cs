namespace Ihc.WebApi.Model;

/// <summary>
/// Contains project identification, revision, and IHC Visual version information.
/// </summary>
public class ProjectInfo
{
    /// <summary>Gets the minor version of IHC Visual used by the project.</summary>
    public required int VisualMinorVersion { get; init; }

    /// <summary>Gets the major version of IHC Visual used by the project.</summary>
    public required int VisualMajorVersion { get; init; }

    /// <summary>Gets the major revision number of the project.</summary>
    public required int ProjectMajorRevision { get; init; }

    /// <summary>Gets the minor revision number of the project.</summary>
    public required int ProjectMinorRevision { get; init; }

    /// <summary>Gets the date and time the project was last modified, if available.</summary>
    public DateTimeOffset? Lastmodified { get; init; }

    /// <summary>Gets the project's identifying number.</summary>
    public required string ProjectNumber { get; init; }

    /// <summary>Gets the customer name recorded in the project.</summary>
    public required string CustomerName { get; init; }

    /// <summary>Gets the installer name recorded in the project.</summary>
    public required string InstallerName { get; init; }
}
