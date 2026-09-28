using System.Xml.Linq;

namespace Ihc.Project.Model;

/// <summary>
/// Represents a product connected through the IHC dataline.
/// </summary>
/// <param name="node">The XML element containing the product.</param>
/// <param name="group">The group that contains the product.</param>
public sealed class DatalineProduct(XElement node, Group group)
    : Product(node, group)
{ }
