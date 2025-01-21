using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

public class Resource : BaseObject
{
    public List<Link> Links { get; protected set; }

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
