namespace Ihc.WebApi.Model;

public class ProjectInfo
{
    public required int VisualMinorVersion { get; init; }

    public required int VisualMajorVersion { get; init; }

    public required int ProjectMajorRevision { get; init; }

    public required int ProjectMinorRevision { get; init; }

    public DateTimeOffset? Lastmodified { get; init; }

    public required string ProjectNumber { get; init; }

    public required string CustomerName { get; init; }

    public required string InstallerName { get; init; }
}
