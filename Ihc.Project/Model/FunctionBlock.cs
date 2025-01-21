using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

public class FunctionBlock : BaseObject
{
    public List<InputResource> Inputs { get; protected set; }

    public List<OutputResource> Outputs { get; protected set; }

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
