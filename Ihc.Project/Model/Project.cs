using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Ihc.Project.Model;

/// <summary>
/// Parses and exposes groups and objects from an IHC project XML document.
/// </summary>
public sealed class Project
{
    /// <summary>
    /// Initializes a project by parsing its groups from an XML document.
    /// </summary>
    /// <param name="xml">The XML document containing an IHC project.</param>
    public Project(XDocument xml)
    {
        Xml = xml;

        foreach (var element in Xml.Element((XName)"utcs_project")?.Element((XName)"groups")?.Elements((XName)"group") ?? [])
        {
            var group = new Group(element, this);
            Groups.Add(group);
            AddObjectMapping(group.Id, group);
        }
    }

    /// <summary>
    /// Parses an IHC project from its XML text.
    /// </summary>
    /// <param name="xml">The XML text containing an IHC project.</param>
    public Project(string xml)
      : this(XDocument.Parse(xml))
    { }

    /// <summary>
    /// The default modification date and time used when the project does not specify a valid modification timestamp.
    /// </summary>
    [JsonIgnore]
    public static readonly DateTime DefaultModified = new(2000, 1, 1, 0, 0, 0, DateTimeKind.Local);

    /// <summary>
    /// The top-level groups contained in the project.
    /// </summary>
    [JsonIgnore]
    public List<Group> Groups { get; } = [];

    /// <summary>
    /// The source XML document.
    /// </summary>
    [JsonIgnore]
    public XDocument Xml { get; }

    /// <summary>
    /// Project objects indexed by their numeric identifiers.
    /// </summary>
    public Dictionary<int, BaseObject> ObjectMap { get; } = [];

    /// <summary>
    /// The project modification date and time.
    /// </summary>
    public DateTime LastModified
    {
        get
        {
            var element = Xml.Element((XName)"utcs_project")?.Element((XName)"modified");
            if (element == null)
            {
                return DefaultModified;
            }

            var year = ParseIntAttribute(element, "year");
            var month = ParseIntAttribute(element, "month");
            var day = ParseIntAttribute(element, "day");
            var hour = ParseIntAttribute(element, "hour");
            var minute = ParseIntAttribute(element, "minute");

            if (year == null || month == null || day == null || hour == null || minute == null)
            {
                return DefaultModified;
            }

            try
            {
                return new DateTime(year.Value, month.Value, day.Value, hour.Value, minute.Value, 0, DateTimeKind.Local);
            }
            catch
            {
                return DefaultModified;
            }
        }
    }

    /// <summary>
    /// Adds an object to the identifier map if that identifier is not already present.
    /// </summary>
    /// <param name="id">The numeric object identifier.</param>
    /// <param name="obj">The object to associate with the identifier.</param>
    public void AddObjectMapping(int id, BaseObject obj)
    {
        if (ObjectMap.ContainsKey(id))
        {
            return;
        }

        ObjectMap.Add(id, obj);
    }

    private static int? ParseIntAttribute(XElement element, string attributeName)
    {
        if (element == null)
        {
            return null;
        }

        var attributeValue = element.Attribute((XName)attributeName)?.Value;
        return int.TryParse(attributeValue, out var result) ? result : null;
    }
}
