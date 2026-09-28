using System.Xml.Linq;

namespace Ihc.Project.Model;

/// <summary>
/// Represents an input resource parsed from an IHC project.
/// </summary>
/// <param name="node">The XML element containing the input resource.</param>
/// <param name="parent">The parent object that contains the resource.</param>
public sealed class InputResource(XElement node, BaseObject parent)
    : Resource(node, parent)
{ }
