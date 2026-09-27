using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

/// <summary>
/// Represents a product connected through the IHC dataline.
/// </summary>
/// <param name="node">The XML element containing the product.</param>
/// <param name="group">The group that contains the product.</param>
public class DatalineProduct(XElement node, Group group)
    : Product(node, group)
{
}
