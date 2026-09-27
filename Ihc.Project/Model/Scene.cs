using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

/// <summary>
/// Represents a scene associated with a product.
/// </summary>
/// <param name="node">The XML element containing the scene.</param>
/// <param name="product">The product that contains the scene.</param>
public class Scene(XElement node, Product product)
    : BaseObject(node, product)
{
}
