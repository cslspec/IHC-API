using System.Text.Json.Serialization;
using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

/// <summary>
/// Parses and exposes groups and objects from an IHC project XML document.
/// </summary>
public class Project
{
    /// <summary>Gets the top-level groups contained in the project.</summary>
    public List<Group> Groups;

    /// <summary>Gets the source XML document.</summary>
    [JsonIgnore]
    public XDocument Xml { get; protected set; }

    /// <summary>Gets the project objects indexed by their numeric identifiers.</summary>
    public Dictionary<int, BaseObject> ObjectMap { get; protected set; }

    /// <summary>
    /// Initializes a project by parsing its groups from an XML document.
    /// </summary>
    /// <param name="xml">The XML document containing an IHC project.</param>
    public Project(XDocument xml)
    {
        this.Xml = xml;
        this.Groups = new List<Group>();
        this.ObjectMap = new Dictionary<int, BaseObject>();
        foreach (XElement element in this.Xml.Element((XName)"utcs_project").Element((XName)"groups").Elements((XName)"group"))
        {
            Group group = new Group(element, this);
            this.Groups.Add(group);
            this.AddObjectMapping(group.Id, (BaseObject)group);
        }
    }

    /// <summary>
    /// Parses an IHC project from its XML text.
    /// </summary>
    /// <param name="xml">The XML text containing an IHC project.</param>
    public Project(string xml)
      : this(XDocument.Parse(xml))
    {
    }

    /// <summary>
    /// Adds an object to the identifier map if that identifier is not already present.
    /// </summary>
    /// <param name="id">The numeric object identifier.</param>
    /// <param name="obj">The object to associate with the identifier.</param>
    public void AddObjectMapping(int id, BaseObject obj)
    {
        if (this.ObjectMap.ContainsKey(id))
            return;
        this.ObjectMap.Add(id, obj);
    }

    /// <summary>Gets the project modification date and time stored in the XML.</summary>
    public DateTime LastModified
    {
        get
        {
            XElement xelement = this.Xml.Element((XName)"utcs_project").Element((XName)"modified");
            return new DateTime(int.Parse(xelement.Attribute((XName)"year").Value), int.Parse(xelement.Attribute((XName)"month").Value), int.Parse(xelement.Attribute((XName)"day").Value), int.Parse(xelement.Attribute((XName)"hour").Value), int.Parse(xelement.Attribute((XName)"minute").Value), 0);
        }
    }
}
