using ProjectModel = Ihc.Project.Model.Project;

namespace Ihc.WebApi.Tests.Model;

[TestClass]
public class ProjectModelTests
{
    [TestMethod]
    public void Constructor_ParsesGroupsProductsPathsAndObjectMappings()
    {
        var project = new ProjectModel(
            """
            <utcs_project>
              <groups>
                <group id="0x00000001" name="Ground floor">
                  <product_dataline id="0x00000002" name="Switch" />
                </group>
              </groups>
            </utcs_project>
            """);

        Assert.AreEqual(1, project.Groups.Count);
        Assert.AreEqual("Ground floor", project.Groups[0].Name);
        Assert.AreEqual(1, project.Groups[0].Products.Count);
        Assert.AreEqual("Ground floor/Switch", project.Groups[0].Products[0].Path);
        Assert.AreSame(project.Groups[0], project.ObjectMap[1]);
        Assert.AreSame(project.Groups[0].Products[0], project.ObjectMap[2]);
    }

    [TestMethod]
    public void LastModified_ParsesTimestampAndSetsLocalKind()
    {
        var project = new ProjectModel(
            """
            <utcs_project>
              <modified year="2025" month="3" day="14" hour="9" minute="26" />
            </utcs_project>
            """);

        Assert.AreEqual(
            new DateTime(2025, 3, 14, 9, 26, 0, DateTimeKind.Local),
            project.LastModified);
    }

    [TestMethod]
    public void LastModified_ReturnsDefaultWhenTimestampIsMissingOrInvalid()
    {
        var missing = new ProjectModel("<utcs_project />");
        var invalid = new ProjectModel(
            """
            <utcs_project>
              <modified year="not-a-year" month="3" day="14" hour="9" minute="26" />
            </utcs_project>
            """);

        Assert.AreEqual(ProjectModel.DefaultModified, missing.LastModified);
        Assert.AreEqual(ProjectModel.DefaultModified, invalid.LastModified);
    }

    [TestMethod]
    public void AddObjectMapping_DoesNotReplaceAnExistingIdentifier()
    {
        var project = new ProjectModel(
            """
            <utcs_project>
              <groups>
                <group id="0x00000001" name="First" />
                <group id="0x00000002" name="Second" />
              </groups>
            </utcs_project>
            """);
        var original = project.Groups[0];

        project.AddObjectMapping(1, project.Groups[1]);

        Assert.AreSame(original, project.ObjectMap[1]);
    }
}
