using System.Xml.Linq;

namespace Ihc.Project.Model;

/// <summary>
/// Represents a resource and the links connected to it.
/// </summary>
public abstract class Resource : BaseObject
{
    /// <summary>
    /// Links to and from this resource.
    /// </summary>
    public List<Link> Links { get; } = [];

    /// <summary>
    /// Initializes a resource from its XML element and parent object.
    /// </summary>
    /// <param name="node">The XML element containing the resource.</param>
    /// <param name="parent">The parent object that contains the resource.</param>
    protected Resource(XElement node, BaseObject parent)
      : base(node, parent)
    {
        foreach (var element in node.Elements((XName)"link_to_resource"))
        {
            Links.Add(new Link(element, this));
        }

        foreach (var element in node.Elements((XName)"link_from_resource"))
        {
            Links.Add(new Link(element, this));
        }
    }
}
