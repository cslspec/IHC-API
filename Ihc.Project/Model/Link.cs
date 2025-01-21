using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

public class Link(XElement node, Resource resource)
    : BaseObject(node, resource)
{
    public Resource Resource => this.Parent as Resource;

    public int LinkId
    {
        get => Convert.ToInt32(this.XmlNode.Attribute((XName)"link").Value.Substring(3), 16);
    }
}
