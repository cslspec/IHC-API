using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Ihc.Project.Model;

/// <summary>
/// Represents a named object parsed from an IHC project XML document.
/// </summary>
public abstract class BaseObject
{
    /// <summary>
    /// Parent object in the project hierarchy.
    /// </summary>
    [JsonIgnore]
    public BaseObject? Parent { get; }

    /// <summary>
    /// Project that contains this object.
    /// </summary>
    [JsonIgnore]
    public virtual Project? Project => Parent?.Project;

    /// <summary>
    /// Slash-delimited path of this object in the project hierarchy.
    /// </summary>
    public string Path => (Parent != null ? Parent.Path + "/" : string.Empty) + Name;

    /// <summary>
    /// Numeric object identifier parsed from the XML <c>id</c> attribute.
    /// Zero when the attribute is missing or invalid.
    /// </summary>
    public int Id => Convert.ToInt32(XmlNode.Attribute((XName)"id")?.Value[3..] ?? "0", 16);

    /// <summary>
    /// Object's name from the XML <c>name</c> attribute.
    /// Empty string when the attribute is missing.
    /// </summary>
    public string Name
    {
        get => XmlNode.Attribute((XName)"name")?.Value ?? string.Empty;
        set => XmlNode.Attribute((XName)"name")?.Value = value;
    }

    /// <summary>
    /// Object's icon identifier, or zero when no icon is specified.
    /// </summary>
    public int Icon
    {
        get
        {
            var xattribute = XmlNode.Attribute((XName)"icon");
            return xattribute == null ? 0 : Convert.ToInt32(xattribute.Value[3..], 16);
        }
    }

    /// <summary>
    /// Runtime model type name of this object.
    /// </summary>
    public string ObjectType => GetType().Name;

    /// <summary>
    /// XML element from which this object was parsed.
    /// </summary>
    [JsonIgnore]
    protected XElement XmlNode { get; set; }

    /// <summary>
    /// Initializes an object from its XML element and parent object.
    /// </summary>
    /// <param name="node">The XML element containing this object's data.</param>
    /// <param name="parent">The parent object, or <see langword="null"/> for a root object.</param>
    protected BaseObject(XElement node, BaseObject? parent)
    {
        XmlNode = node;
        Parent = parent;

        if (Project == null)
        {
            return;
        }

        Project.AddObjectMapping(Id, this);
    }
}
