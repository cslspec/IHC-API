using Ihc.WebApi.Controllers;
using Ihc.WebApi.Model.Time;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Ihc.WebApi.Tests.Controllers;

[TestClass]
public class TimeControllerTests
{
    private ITimeService service = null!;
    private TimeController controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        service = Substitute.For<ITimeService>();
        controller = new TimeController(service);
    }

    [TestMethod]
    public async Task GetUptime_ReturnsServiceValue()
    {
        var expected = new Uptime
        {
            Time = TimeSpan.FromMilliseconds(1234),
            Days = 0,
            Hours = 0,
            Minutes = 0,
            Seconds = 1,
            Milliseconds = 234,
            TotalMilliseconds = 1234
        };
        service.GetUptime().Returns(expected);

        var result = await controller.GetUptime();

        Assert.AreSame(expected, result.Value);
        await service.Received(1).GetUptime();
    }

    [TestMethod]
    public async Task GetLocalTime_WrapsServiceValue()
    {
        var expected = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Local);
        service.GetLocalTime().Returns(expected);

        var result = await controller.GetLocalTime();

        Assert.IsNotNull(result.Value);
        Assert.AreEqual(expected, result.Value.CurrentLocalTime);
        await service.Received(1).GetLocalTime();
    }

    [TestMethod]
    public async Task UpdateSettings_ReturnsNoContentAndForwardsRequest()
    {
        var request = new UpdateTimeSettingsRequest { TimeServerName = "time.example.com" };

        var result = await controller.UpdateSettings(request);

        Assert.IsInstanceOfType<NoContentResult>(result);
        await service.Received(1).UpdateSettings(Arg.Is<UpdateTimeSettingsRequest>(value => ReferenceEquals(value, request)));
    }

    [TestMethod]
    public async Task GetTimeFromServer_ForwardsServerName()
    {
        var expected = new TimeServerConnectionResult
        {
            ConnectionWasSuccessful = true,
            ConnectionFailedDueToUnknownHost = false,
            ConnectionFailedDueToOtherErrors = false,
            Milliseconds = 1234,
            Time = DateTime.UnixEpoch
        };
        service.GetTimeFromServer("time.example.com").Returns(expected);

        var result = await controller.GetTimeFromServer("time.example.com");

        Assert.AreSame(expected, result.Value);
        await service.Received(1).GetTimeFromServer("time.example.com");
    }
}
