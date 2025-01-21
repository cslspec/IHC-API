using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

public class Product : BaseObject
{
    public List<Resource> Resources { get; } = [];

    public List<Scene> Scenes { get; } = [];

    public string Note
    {
        get => this.XmlNode.Attribute((XName)"note").Value;
        set => this.XmlNode.Attribute((XName)"note").Value = value;
    }

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
            if (element.Attribute((XName)"settings") == null || !(element.Attribute((XName)"setting").Value == "yes"))
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
