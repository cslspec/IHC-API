using System.Text.Json.Serialization;
using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

/// <summary>
/// Represents a named object parsed from an IHC project XML document.
/// </summary>
public class BaseObject
{
    /// <summary>
    /// Initializes an object from its XML element and parent object.
    /// </summary>
    /// <param name="node">The XML element containing this object's data.</param>
    /// <param name="parent">The parent object, or <see langword="null"/> for a root object.</param>
    public BaseObject(XElement node, BaseObject parent)
    {
        this.XmlNode = node;
        this.Parent = parent;
        if (this.Project == null)
            return;
        this.Project.AddObjectMapping(this.Id, this);
    }

    /// <summary>Gets the parent object in the project hierarchy.</summary>
    [JsonIgnore]
    public BaseObject Parent { get; protected set; }

    /// <summary>Gets the project that contains this object.</summary>
    [JsonIgnore]
    public virtual Project Project => this.Parent.Project;

    /// <summary>Gets the slash-delimited path of this object in the project hierarchy.</summary>
    public string Path => (this.Parent != null ? this.Parent.Path + "/" : "") + this.Name;

    /// <summary>Gets or sets the XML element from which this object was parsed.</summary>
    [JsonIgnore]
    protected XElement XmlNode { get; set; }

    /// <summary>Gets the numeric object identifier parsed from the XML <c>id</c> attribute.</summary>
    public int Id => Convert.ToInt32(this.XmlNode.Attribute((XName)"id").Value.Substring(3), 16);

    /// <summary>Gets or sets the object's name from the XML <c>name</c> attribute.</summary>
    public string Name
    {
        get => this.XmlNode.Attribute((XName)"name").Value;
        set => this.XmlNode.Attribute((XName)"name").Value = value;
    }

    /// <summary>Gets the object's icon identifier, or zero when no icon is specified.</summary>
    public int Icon
    {
        get
        {
            XAttribute xattribute = this.XmlNode.Attribute((XName)"icon");
            return xattribute == null ? 0 : Convert.ToInt32(xattribute.Value.Substring(3), 16);
        }
    }

    /// <summary>Gets the runtime model type name of this object.</summary>
    public string ObjectType => this.GetType().Name;
}
