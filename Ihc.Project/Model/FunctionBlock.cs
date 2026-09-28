using System.Xml.Linq;

namespace Ihc.Project.Model;

/// <summary>
/// Represents a function block and its input and output resources.
/// </summary>
public sealed class FunctionBlock : BaseObject
{
    /// <summary>
    /// Input resources of this function block.
    /// </summary>
    public List<InputResource> Inputs { get; } = [];

    /// <summary>
    /// Output resources of this function block.
    /// </summary>
    public List<OutputResource> Outputs { get; } = [];

    /// <summary>
    /// Initializes a function block from its XML element and containing group.
    /// </summary>
    /// <param name="node">The XML element containing the function block.</param>
    /// <param name="group">The group that contains the function block.</param>
    public FunctionBlock(XElement node, Group group)
      : base(node, group)
    {
        foreach (var element in node.Elements((XName)"inputs").Elements<XElement>())
        {
            Inputs.Add(new InputResource(element, this));
        }

        foreach (var element in node.Elements((XName)"outputs").Elements<XElement>())
        {
            Outputs.Add(new OutputResource(element, this));
        }
    }
}
