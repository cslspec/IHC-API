using Ihc.WebApi.Controllers;
using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Ihc.WebApi.Tests.Controllers;

[TestClass]
public class ConfigControllerTests
{
    private IConfigurationService configService = null!;
    private IAccessControlService accessService = null!;
    private IAuthCacheService authCacheService = null!;
    private ConfigController controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        configService = Substitute.For<IConfigurationService>();
        accessService = Substitute.For<IAccessControlService>();
        authCacheService = Substitute.For<IAuthCacheService>();
        controller = new ConfigController(configService, accessService, authCacheService);
    }

    [TestMethod]
    public async Task GetEmailEnableSettings_WrapsServiceValue()
    {
        configService.GetEmailEnableSettings().Returns(true);

        var result = await controller.GetEmailEnableSettings();

        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.IsEmailEnabled);
        await configService.Received(1).GetEmailEnableSettings();
    }

    [TestMethod]
    public async Task GetAccessControl_ReturnsServiceValue()
    {
        var expected = new[]
        {
            new AccessControlSetting
            {
                Id = 1,
                Name = "Viewer",
                Description = "Read-only access",
                Usb = false,
                Internal = true,
                External = false
            }
        };
        accessService.GetAccessControl().Returns(expected);

        var result = await controller.GetAccessControl();

        Assert.AreSame(expected, result.Value);
        await accessService.Received(1).GetAccessControl();
    }

    [TestMethod]
    public async Task UpdateSmtpSettings_ReturnsNoContentAndForwardsRequest()
    {
        var request = new SmtpSettings { ServerAddress = "smtp.example.com", ServerPortNumber = 25 };

        var result = await controller.UpdateSmtpSettings(request);

        Assert.IsInstanceOfType<NoContentResult>(result);
        await configService.Received(1).UpdateSmtpSettings(Arg.Is<SmtpSettings>(value => ReferenceEquals(value, request)));
    }

    [TestMethod]
    public void Logout_ClearsCacheAndReturnsNoContent()
    {
        var result = controller.Logout();

        Assert.IsInstanceOfType<NoContentResult>(result);
        authCacheService.Received(1).ClearCache();
    }
}
