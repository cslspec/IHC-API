using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

public class DatalineProduct(XElement node, Group group)
    : Product(node, group)
{
}
