using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

/// <summary>
/// Represents a top-level group and its products and function blocks.
/// </summary>
public class Group : BaseObject
{
    private Project _Project;

    /// <inheritdoc />
    public override Project Project => this._Project;

    /// <summary>Gets the products contained in this group.</summary>
    public List<Product> Products { get; protected set; }

    /// <summary>Gets the function blocks contained in this group.</summary>
    public List<FunctionBlock> FunctionBlocks { get; protected set; }

    /// <summary>
    /// Initializes a group from its XML element and containing project.
    /// </summary>
    /// <param name="node">The XML element containing the group.</param>
    /// <param name="project">The project that contains the group.</param>
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
