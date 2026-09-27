using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

/// <summary>
/// Represents a wireless product parsed from an IHC project.
/// </summary>
/// <param name="node">The XML element containing the product.</param>
/// <param name="group">The group that contains the product.</param>
public class WirelessProduct(XElement node, Group group)
    : Product(node, group)
{
}
