using System.Xml.Linq;

namespace Ihc.Project.Model;

/// <summary>
/// Represents a link between resources in an IHC project.
/// </summary>
/// <param name="node">The XML element containing the link.</param>
/// <param name="resource">The resource associated with the link.</param>
public sealed class Link(XElement node, Resource resource)
    : BaseObject(node, resource)
{
    /// <summary>
    /// The resource associated with this link.
    /// </summary>
    public Resource? Resource => Parent as Resource;

    /// <summary>
    /// The identifier of the linked resource.
    /// </summary>
    public int LinkId => Convert.ToInt32(XmlNode.Attribute((XName)"link")?.Value[3..], 16);
}
