using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

/// <summary>
/// Represents a product and its resources and scenes.
/// </summary>
public class Product : BaseObject
{
    /// <summary>Gets the resources provided by this product.</summary>
    public List<Resource> Resources { get; } = [];

    /// <summary>Gets the scenes associated with this product.</summary>
    public List<Scene> Scenes { get; } = [];

    /// <summary>Gets or sets the product note stored in the XML.</summary>
    public string Note
    {
        get => this.XmlNode.Attribute((XName)"note").Value;
        set => this.XmlNode.Attribute((XName)"note").Value = value;
    }

    /// <summary>
    /// Initializes a product from its XML element and containing group.
    /// </summary>
    /// <param name="node">The XML element containing the product.</param>
    /// <param name="group">The group that contains the product.</param>
    public Product(XElement node, Group group)
      : base(node, group)
    {

        foreach (XElement element in node.Elements((XName)"airlink_input"))
            this.Resources.Add((Resource)new InputResource(element, (BaseObject)this));
        foreach (XElement element in node.Elements((XName)"airlink_dimmer_increase"))
            this.Resources.Add((Resource)new InputResource(element, (BaseObject)this));
        foreach (XElement element in node.Elements((XName)"airlink_dimmer_decrease"))
            this.Resources.Add((Resource)new InputResource(element, (BaseObject)this));
        foreach (XElement element in node.Elements((XName)"airlink_dimming"))
            this.Resources.Add((Resource)new InputResource(element, (BaseObject)this));
        foreach (XElement element in node.Elements((XName)"dataline_input"))
            this.Resources.Add((Resource)new InputResource(element, (BaseObject)this));

        foreach (XElement element in node.Elements((XName)"airlink_relay"))
            this.Resources.Add((Resource)new OutputResource(element, (BaseObject)this));
        foreach (XElement element in node.Elements((XName)"dataline_output"))
            this.Resources.Add((Resource)new OutputResource(element, (BaseObject)this));
        foreach (XElement element in node.Elements((XName)"light_indication"))
            this.Resources.Add((Resource)new OutputResource(element, (BaseObject)this));

        foreach (XElement element in node.Elements((XName)"resource_temperature"))
        {
            if (element.Attribute((XName)"settings")?.Value != "yes")
                this.Resources.Add((Resource)new OutputResource(element, (BaseObject)this));
        }

        foreach (XElement element in node.Elements((XName)"resource_humidity_level"))
            this.Resources.Add((Resource)new OutputResource(element, (BaseObject)this));

        foreach (XElement element in node.Elements((XName)"resource_light"))
            this.Resources.Add((Resource)new OutputResource(element, (BaseObject)this));

        foreach (XElement element in node.Elements((XName)"scenes").Elements<XElement>())
            this.Scenes.Add(new Scene(element, this));
    }
}
