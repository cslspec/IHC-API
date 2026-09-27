using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

/// <summary>
/// Represents a link between resources in an IHC project.
/// </summary>
/// <param name="node">The XML element containing the link.</param>
/// <param name="resource">The resource associated with the link.</param>
public class Link(XElement node, Resource resource)
    : BaseObject(node, resource)
{
    /// <summary>Gets the resource associated with this link.</summary>
    public Resource Resource => this.Parent as Resource;

    /// <summary>Gets the identifier of the linked resource.</summary>
    public int LinkId
    {
        get => Convert.ToInt32(this.XmlNode.Attribute((XName)"link").Value.Substring(3), 16);
    }
}
