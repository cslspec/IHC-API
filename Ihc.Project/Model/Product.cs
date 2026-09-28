using System.Xml.Linq;

namespace Ihc.Project.Model;

/// <summary>
/// Represents a product and its resources and scenes.
/// </summary>
public abstract class Product : BaseObject
{
    /// <summary>
    /// Resources provided by this product.
    /// </summary>
    public List<Resource> Resources { get; } = [];

    /// <summary>
    /// Scenes associated with this product.
    /// </summary>
    public List<Scene> Scenes { get; } = [];

    /// <summary>
    /// Product note.
    /// </summary>
    public string Note
    {
        get => XmlNode.Attribute((XName)"note")?.Value ?? string.Empty;
        set => XmlNode.Attribute((XName)"note")?.Value = value;
    }

    /// <summary>
    /// Initializes a product from its XML element and containing group.
    /// </summary>
    /// <param name="node">The XML element containing the product.</param>
    /// <param name="group">The group that contains the product.</param>
    protected Product(XElement node, Group group)
      : base(node, group)
    {

        foreach (var element in node.Elements((XName)"airlink_input"))
        {
            Resources.Add(new InputResource(element, this));
        }

        foreach (var element in node.Elements((XName)"airlink_dimmer_increase"))
        {
            Resources.Add(new InputResource(element, this));
        }

        foreach (var element in node.Elements((XName)"airlink_dimmer_decrease"))
        {
            Resources.Add(new InputResource(element, this));
        }

        foreach (var element in node.Elements((XName)"airlink_dimming"))
        {
            Resources.Add(new InputResource(element, this));
        }

        foreach (var element in node.Elements((XName)"dataline_input"))
        {
            Resources.Add(new InputResource(element, this));
        }

        foreach (var element in node.Elements((XName)"airlink_relay"))
        {
            Resources.Add(new OutputResource(element, this));
        }

        foreach (var element in node.Elements((XName)"dataline_output"))
        {
            Resources.Add(new OutputResource(element, this));
        }

        foreach (var element in node.Elements((XName)"light_indication"))
        {
            Resources.Add(new OutputResource(element, this));
        }

        foreach (var element in node.Elements((XName)"resource_temperature"))
        {
            if (element.Attribute((XName)"settings")?.Value != "yes")
            {
                Resources.Add(new OutputResource(element, this));
            }
        }

        foreach (var element in node.Elements((XName)"resource_humidity_level"))
        {
            Resources.Add(new OutputResource(element, this));
        }

        foreach (var element in node.Elements((XName)"resource_light"))
        {
            Resources.Add(new OutputResource(element, this));
        }

        foreach (var element in node.Elements((XName)"scenes").Elements())
        {
            Scenes.Add(new Scene(element, this));
        }
    }
}
