using Ihc.WebApi.Controllers;
using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using NSubstitute;

namespace Ihc.WebApi.Tests.Controllers;

[TestClass]
public class ProjectControllerTests
{
    private IProjectService service = null!;
    private ProjectController controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        service = Substitute.For<IProjectService>();
        controller = new ProjectController(service);
    }

    [TestMethod]
    public async Task GetIsProjectAvailable_WrapsServiceValue()
    {
        service.GetIsProjectAvailable().Returns(true);

        var result = await controller.GetIsProjectAvailable();

        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.IsProjectAvailable);
        await service.Received(1).GetIsProjectAvailable();
    }

    [TestMethod]
    public async Task GetProjectInfo_ReturnsServiceValue()
    {
        var expected = new ProjectInfo
        {
            CustomerName = "Customer",
            InstallerName = "Installer",
            ProjectNumber = "P-123",
            ProjectMajorRevision = 1,
            ProjectMinorRevision = 2,
            VisualMajorVersion = 3,
            VisualMinorVersion = 4
        };
        service.GetProjectInfo().Returns(expected);

        var result = await controller.GetProjectInfo();

        Assert.AreSame(expected, result.Value);
        await service.Received(1).GetProjectInfo();
    }

    [TestMethod]
    public async Task GetProjectFile_ReturnsContentWithCalculatedSize()
    {
        const string expectedContent = "test";
        service.GetProjectFile().Returns(expectedContent);

        var result = await controller.GetProjectFile();

        Assert.IsNotNull(result.Value);
        Assert.AreEqual(expectedContent.Length, result.Value.Size);
        Assert.AreEqual(expectedContent, result.Value.Content);
        await service.Received(1).GetProjectFile();
    }
}
