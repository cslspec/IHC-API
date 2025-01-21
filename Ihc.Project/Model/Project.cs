using System.Text.Json.Serialization;
using System.Xml.Linq;

#nullable disable
namespace Ihc.Project.Model;

public class Project
{
    public List<Group> Groups;

    [JsonIgnore]
    public XDocument Xml { get; protected set; }

    public Dictionary<int, BaseObject> ObjectMap { get; protected set; }

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

    public Project(string xml)
      : this(XDocument.Parse(xml))
    {
    }

    public void AddObjectMapping(int id, BaseObject obj)
    {
        if (this.ObjectMap.ContainsKey(id))
            return;
        this.ObjectMap.Add(id, obj);
    }

    public DateTime LastModified
    {
        get
        {
            XElement xelement = this.Xml.Element((XName)"utcs_project").Element((XName)"modified");
            return new DateTime(int.Parse(xelement.Attribute((XName)"year").Value), int.Parse(xelement.Attribute((XName)"month").Value), int.Parse(xelement.Attribute((XName)"day").Value), int.Parse(xelement.Attribute((XName)"hour").Value), int.Parse(xelement.Attribute((XName)"minute").Value), 0);
        }
    }
}
