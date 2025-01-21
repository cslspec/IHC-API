using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

public class Scene(XElement node, Product product)
    : BaseObject(node, product)
{
}
