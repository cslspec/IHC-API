using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

public class WirelessProduct(XElement node, Group group)
    : Product(node, group)
{
}
