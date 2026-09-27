using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

/// <summary>
/// Represents a function block and its input and output resources.
/// </summary>
public class FunctionBlock : BaseObject
{
    /// <summary>Gets the input resources of this function block.</summary>
    public List<InputResource> Inputs { get; protected set; }

    /// <summary>Gets the output resources of this function block.</summary>
    public List<OutputResource> Outputs { get; protected set; }

    /// <summary>
    /// Initializes a function block from its XML element and containing group.
    /// </summary>
    /// <param name="node">The XML element containing the function block.</param>
    /// <param name="group">The group that contains the function block.</param>
    public FunctionBlock(XElement node, Group group)
      : base(node, (BaseObject)group)
    {
        this.Inputs = new List<InputResource>();
        this.Outputs = new List<OutputResource>();
        foreach (XElement element in node.Elements((XName)"inputs").Elements<XElement>())
            this.Inputs.Add(new InputResource(element, (BaseObject)this));
        foreach (XElement element in node.Elements((XName)"outputs").Elements<XElement>())
            this.Outputs.Add(new OutputResource(element, (BaseObject)this));
    }
}
