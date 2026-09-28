using System.Xml.Linq;

namespace Ihc.Project.Model;

/// <summary>
/// Represents a top-level group and its products and function blocks.
/// </summary>
public sealed class Group : BaseObject
{
    /// <inheritdoc />
    public override Project? Project { get; }

    /// <summary>
    /// Products contained in this group.
    /// </summary>
    public List<Product> Products { get; } = [];

    /// <summary>
    /// Function blocks contained in this group.
    /// </summary>
    public List<FunctionBlock> FunctionBlocks { get; } = [];

    /// <summary>
    /// Initializes a group from its XML element and containing project.
    /// </summary>
    /// <param name="node">The XML element containing the group.</param>
    /// <param name="project">The project that contains the group.</param>
    public Group(XElement node, Project project)
      : base(node, null)
    {
        Project = project;

        foreach (var element in node.Elements((XName)"product_airlink"))
        {
            Products.Add(new WirelessProduct(element, this));
        }

        foreach (var element in node.Elements((XName)"product_dataline"))
        {
            Products.Add(new DatalineProduct(element, this));
        }

        foreach (var element in node.Elements((XName)"functionblock"))
        {
            FunctionBlocks.Add(new FunctionBlock(element, this));
        }
    }
}
