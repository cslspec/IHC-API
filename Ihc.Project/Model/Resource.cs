using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

/// <summary>
/// Represents a resource and the links connected to it.
/// </summary>
public class Resource : BaseObject
{
    /// <summary>Gets the links to and from this resource.</summary>
    public List<Link> Links { get; protected set; }

    /// <summary>
    /// Initializes a resource from its XML element and parent object.
    /// </summary>
    /// <param name="node">The XML element containing the resource.</param>
    /// <param name="parent">The parent object that contains the resource.</param>
    public Resource(XElement node, BaseObject parent)
      : base(node, parent)
    {
        this.Links = new List<Link>();
        foreach (XElement element in node.Elements((XName)"link_to_resource"))
            this.Links.Add(new Link(element, this));

        foreach (XElement element in node.Elements((XName)"link_from_resource"))
            this.Links.Add(new Link(element, this));
    }
}
