using System.Text.Json.Serialization;
using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

public class BaseObject
{
    public BaseObject(XElement node, BaseObject parent)
    {
        this.XmlNode = node;
        this.Parent = parent;
        if (this.Project == null)
            return;
        this.Project.AddObjectMapping(this.Id, this);
    }

    [JsonIgnore]
    public BaseObject Parent { get; protected set; }

    [JsonIgnore]
    public virtual Project Project => this.Parent.Project;

    public string Path => (this.Parent != null ? this.Parent.Path + "/" : "") + this.Name;

    [JsonIgnore]
    protected XElement XmlNode { get; set; }

    public int Id => Convert.ToInt32(this.XmlNode.Attribute((XName)"id").Value.Substring(3), 16);

    public string Name
    {
        get => this.XmlNode.Attribute((XName)"name").Value;
        set => this.XmlNode.Attribute((XName)"name").Value = value;
    }

    public int Icon
    {
        get
        {
            XAttribute xattribute = this.XmlNode.Attribute((XName)"icon");
            return xattribute == null ? 0 : Convert.ToInt32(xattribute.Value.Substring(3), 16);
        }
    }

    public string ObjectType => this.GetType().Name;
}
