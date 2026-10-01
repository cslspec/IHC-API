using Ihc.Soap.Configuration;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Ihc.WebApi.Tests.Services;

[TestClass]
public class AccessControlServiceTests
{
    private IClientService client = null!;
    private IAuthCacheService authCache = null!;
    private AccessControlService service = null!;

    [TestInitialize]
    public void Initialize()
    {
        client = Substitute.For<IClientService>();
        authCache = Substitute.For<IAuthCacheService>();
        service = new AccessControlService(
            client,
            authCache,
            Substitute.For<ILogger<AccessControlService>>());

        var token = Substitute.For<IAuthToken>();
        token.Token.Returns("session-token");
        authCache.GetAuthToken().Returns(token);
    }

    [TestMethod]
    public async Task GetAccessControl_MapsControllerPermissions()
    {
        var info = new WSAccessControl
        {
            m_administrator_external = true,
            m_administrator_internal = false,
            m_administrator_usb = true,
            m_openapi_external = false,
            m_openapi_internal = true,
            m_openapi_usb = false,
            m_usbLoginRequired_usb = true
        };
        client.Post<inputMessageName14, outputMessageName14>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName14>())
            .Returns(Task.FromResult(new outputMessageName14(info)));

        var result = await service.GetAccessControl();

        Assert.AreEqual(10, result.Length);
        Assert.AreEqual(1, result[0].Id);
        Assert.AreEqual("Administrator", result[0].Name);
        Assert.IsTrue(result[0].External);
        Assert.IsFalse(result[0].Internal);
        Assert.IsTrue(result[0].Usb);
        Assert.AreEqual(4, result[3].Id);
        Assert.AreEqual("Open API", result[3].Name);
        Assert.IsFalse(result[3].External);
        Assert.IsTrue(result[3].Internal);
        Assert.IsFalse(result[3].Usb);
        Assert.AreEqual(10, result[9].Id);
        Assert.AreEqual("USB", result[9].Name);
        Assert.IsFalse(result[9].External);
        Assert.IsFalse(result[9].Internal);
        Assert.IsTrue(result[9].Usb);
        await client.Received(1).Post<inputMessageName14, outputMessageName14>(
            "ConfigurationService",
            "getWebAccessControl",
            "session-token",
            Arg.Any<inputMessageName14>());
    }

    [TestMethod]
    public async Task GetAccessControl_WhenResponseIsNull_ThrowsEmptyResponseException()
    {
        client.Post<inputMessageName14, outputMessageName14>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName14>())
            .Returns(Task.FromResult<outputMessageName14>(null!));

        await Assert.ThrowsExactlyAsync<EmptyResponseException>(() => service.GetAccessControl());
    }

    [TestMethod]
    public async Task GetAccessControl_WhenSettingsAreNull_ThrowsEmptyResponseException()
    {
        client.Post<inputMessageName14, outputMessageName14>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName14>())
            .Returns(Task.FromResult(new outputMessageName14 { getWebAccessControl1 = null! }));

        await Assert.ThrowsExactlyAsync<EmptyResponseException>(() => service.GetAccessControl());
    }
}
