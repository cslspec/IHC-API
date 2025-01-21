using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

public class Group : BaseObject
{
    private Project _Project;

    public override Project Project => this._Project;

    public List<Product> Products { get; protected set; }

    public List<FunctionBlock> FunctionBlocks { get; protected set; }

    public Group(XElement node, Project project)
      : base(node, (BaseObject)null)
    {
        this._Project = project;
        this.Products = new List<Product>();
        this.FunctionBlocks = new List<FunctionBlock>();
        foreach (XElement element in node.Elements((XName)"product_airlink"))
            this.Products.Add((Product)new WirelessProduct(element, this));
        foreach (XElement element in node.Elements((XName)"product_dataline"))
            this.Products.Add((Product)new DatalineProduct(element, this));
        foreach (XElement element in node.Elements((XName)"functionblock"))
            this.FunctionBlocks.Add(new FunctionBlock(element, this));
    }
}
