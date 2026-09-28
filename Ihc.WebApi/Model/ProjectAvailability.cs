namespace Ihc.WebApi.Model;

/// <summary>
/// Represents the availability status of a project.
/// </summary>
public class ProjectAvailability
{
    /// <summary>
    /// Indicates whether the project is available.
    /// </summary>
    public required bool IsProjectAvailable { get; set; }
}
