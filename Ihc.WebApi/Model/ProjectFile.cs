namespace Ihc.WebApi.Model;

/// <summary>
/// Represents a project file with its size and content.
/// </summary>
public class ProjectFile
{
    /// <summary>
    /// Total size of the project file in characters.
    /// </summary>
    public required int Size { get; set; }

    /// <summary>
    /// The XML content of the project file.
    /// </summary>
    public required string Content { get; set; }
}
