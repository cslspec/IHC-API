using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

public class OutputResource(XElement node, BaseObject parent)
    : Resource(node, parent)
{
}
