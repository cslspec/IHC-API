using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

/// <summary>
/// Represents an output resource parsed from an IHC project.
/// </summary>
/// <param name="node">The XML element containing the output resource.</param>
/// <param name="parent">The parent object that contains the resource.</param>
public class OutputResource(XElement node, BaseObject parent)
    : Resource(node, parent)
{
}
