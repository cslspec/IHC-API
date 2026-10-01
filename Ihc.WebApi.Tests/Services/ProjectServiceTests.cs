using Ihc.Soap.Controller;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using NSubstitute;

namespace Ihc.WebApi.Tests.Services;

[TestClass]
public class ProjectServiceTests
{
    private IClientService client = null!;
    private IAuthCacheService authCache = null!;
    private ISoapDateService dateService = null!;
    private ProjectService service = null!;

    [TestInitialize]
    public void Initialize()
    {
        client = Substitute.For<IClientService>();
        authCache = Substitute.For<IAuthCacheService>();
        dateService = Substitute.For<ISoapDateService>();
        service = new ProjectService(client, authCache, dateService);

        var token = Substitute.For<IAuthToken>();
        token.Token.Returns("session-token");
        authCache.GetAuthToken().Returns(token);
    }

    [TestMethod]
    public async Task GetIsProjectAvailable_ReturnsControllerValue()
    {
        client.Post<inputMessageName4, outputMessageName4>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName4>())
            .Returns(Task.FromResult(new outputMessageName4(true)));

        var result = await service.GetIsProjectAvailable();

        Assert.IsTrue(result);
        await client.Received(1).Post<inputMessageName4, outputMessageName4>(
            "ControllerService",
            "isIHCProjectAvailable",
            "session-token",
            Arg.Any<inputMessageName4>());
    }

    [TestMethod]
    public async Task GetIsProjectAvailable_WhenControllerHasNoResult_ReturnsFalse()
    {
        client.Post<inputMessageName4, outputMessageName4>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName4>())
            .Returns(Task.FromResult<outputMessageName4>(null!));

        var result = await service.GetIsProjectAvailable();

        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task GetProjectInfo_MapsControllerFieldsAndConvertsModifiedDate()
    {
        var modified = new WSDate();
        var soapInfo = new WSProjectInfo
        {
            customerName = "Customer",
            installerName = "Installer",
            projectNumber = "P-123",
            projectMajorRevision = 2,
            projectMinorRevision = 5,
            visualMajorVersion = 3,
            visualMinorVersion = 7,
            lastmodified = modified
        };
        var expectedDate = new DateTime(2024, 6, 15, 12, 30, 0, DateTimeKind.Local);
        client.Post<inputMessageName11, outputMessageName11>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName11>())
            .Returns(Task.FromResult(new outputMessageName11(soapInfo)));
        dateService.GetDateTime(modified).Returns(expectedDate);

        var result = await service.GetProjectInfo();

        Assert.AreEqual("Customer", result.CustomerName);
        Assert.AreEqual("Installer", result.InstallerName);
        Assert.AreEqual("P-123", result.ProjectNumber);
        Assert.AreEqual(2, result.ProjectMajorRevision);
        Assert.AreEqual(5, result.ProjectMinorRevision);
        Assert.AreEqual(3, result.VisualMajorVersion);
        Assert.AreEqual(7, result.VisualMinorVersion);
        Assert.AreEqual(expectedDate, result.Lastmodified);
        dateService.Received(1).GetDateTime(modified);
    }

    [TestMethod]
    public async Task GetProjectInfo_WhenControllerReturnsNoInfo_ThrowsEmptyResponseException()
    {
        client.Post<inputMessageName11, outputMessageName11>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName11>())
            .Returns(Task.FromResult(new outputMessageName11 { getProjectInfo1 = null! }));

        await Assert.ThrowsExactlyAsync<EmptyResponseException>(() => service.GetProjectInfo());
    }
}
