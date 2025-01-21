using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

public class InputResource(XElement node, BaseObject parent)
    : Resource(node, parent)
{
}
