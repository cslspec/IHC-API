using Ihc.Soap.TimeManager;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;
using Ihc.WebApi.Model.Time;
using Ihc.WebApi.Services;
using NSubstitute;

namespace Ihc.WebApi.Tests.Services;

[TestClass]
public class TimeServiceTests
{
    private IClientService client = null!;
    private ISoapDateService dateService = null!;
    private IAuthCacheService authCache = null!;
    private TimeService service = null!;

    [TestInitialize]
    public void Initialize()
    {
        client = Substitute.For<IClientService>();
        dateService = Substitute.For<ISoapDateService>();
        authCache = Substitute.For<IAuthCacheService>();
        service = new TimeService(client, dateService, authCache);

        var token = Substitute.For<IAuthToken>();
        token.Token.Returns("session-token");
        authCache.GetAuthToken().Returns(token);
        dateService.GetDateTime(Arg.Any<WSDate>(), DateTimeKind.Utc)
            .Returns(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));
    }

    [TestMethod]
    public async Task GetUptime_ConvertsTotalMillisecondsToComponents()
    {
        const long totalMilliseconds = 2 * 86_400_000L + 3_723_456;
        client.Post<inputMessageName5, outputMessageName5>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName5>())
            .Returns(Task.FromResult(new outputMessageName5(totalMilliseconds)));

        var result = await service.GetUptime();

        Assert.AreEqual(TimeSpan.FromMilliseconds(totalMilliseconds), result.Time);
        Assert.AreEqual(2, result.Days);
        Assert.AreEqual(1, result.Hours);
        Assert.AreEqual(2, result.Minutes);
        Assert.AreEqual(3, result.Seconds);
        Assert.AreEqual(456, result.Milliseconds);
        Assert.AreEqual(totalMilliseconds, result.TotalMilliseconds);
    }

    [TestMethod]
    public async Task GetUptime_WhenResponseIsEmpty_ThrowsEmptyResponseException()
    {
        client.Post<inputMessageName5, outputMessageName5>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName5>())
            .Returns(Task.FromResult<outputMessageName5>(null!));

        await Assert.ThrowsExactlyAsync<EmptyResponseException>(() => service.GetUptime());
    }

    [TestMethod]
    public async Task GetLocalTime_ConvertsControllerDate()
    {
        var soapDate = new WSDate();
        var expected = new DateTime(2026, 10, 1, 16, 19, 3);
        client.Post<inputMessageName2, outputMessageName2>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName2>())
            .Returns(Task.FromResult(new outputMessageName2(soapDate)));
        dateService.GetDateTime(soapDate).Returns(expected);

        var result = await service.GetLocalTime();

        Assert.AreEqual(expected, result);
        dateService.Received(1).GetDateTime(soapDate);
    }

    [TestMethod]
    public async Task GetLocalTime_WhenControllerReturnsNoDate_ThrowsEmptyResponseException()
    {
        client.Post<inputMessageName2, outputMessageName2>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName2>())
            .Returns(Task.FromResult(new outputMessageName2 { getCurrentLocalTime1 = null! }));

        await Assert.ThrowsExactlyAsync<EmptyResponseException>(() => service.GetLocalTime());
    }

    [TestMethod]
    public async Task GetSettings_MapsControllerSettingsAndConvertsTimeAsUtc()
    {
        var soapDate = new WSDate();
        var expectedDate = new DateTime(2026, 10, 1, 14, 0, 0, DateTimeKind.Utc);
        ConfigureSettings(new WSTimeManagerSettings
        {
            serverName = "time.example.com",
            synchroniseTimeAgainstServer = true,
            syncIntervalInHours = 12,
            gmtOffsetInHours = 2,
            useDST = true,
            timeAndDateInUTC = soapDate
        });
        dateService.GetDateTime(soapDate, DateTimeKind.Utc).Returns(expectedDate);

        var result = await service.GetSettings();

        Assert.AreEqual("time.example.com", result.TimeServerName);
        Assert.AreEqual(true, result.Synchronize);
        Assert.AreEqual(12, result.SynchronizeInterval);
        Assert.AreEqual(2, result.GmtOffset);
        Assert.AreEqual(true, result.UseDst);
        Assert.AreEqual(expectedDate, result.CurrentTime);
        dateService.Received(1).GetDateTime(soapDate, DateTimeKind.Utc);
    }

    [TestMethod]
    public async Task GetSettings_WhenControllerReturnsNoSettings_ThrowsEmptyResponseException()
    {
        client.Post<inputMessageName3, outputMessageName3>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName3>())
            .Returns(Task.FromResult(new outputMessageName3 { getSettings1 = null! }));

        await Assert.ThrowsExactlyAsync<EmptyResponseException>(() => service.GetSettings());
    }

    [TestMethod]
    public async Task GetTimeFromServer_TrimsExplicitServerAndConvertsSuccessfulTime()
    {
        const long unixMilliseconds = 1_798_755_543_000;
        client.Post<inputMessageName1, outputMessageName1>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName1>())
            .Returns(Task.FromResult(new outputMessageName1(new WSTimeServerConnectionResult
            {
                connectionWasSuccessful = true,
                dateFromServer = unixMilliseconds
            })));

        var result = await service.GetTimeFromServer("  time.example.com  ");

        Assert.IsTrue(result.ConnectionWasSuccessful);
        Assert.AreEqual(unixMilliseconds, result.Milliseconds);
        Assert.AreEqual(DateTimeOffset.FromUnixTimeMilliseconds(unixMilliseconds).UtcDateTime, result.Time);
        await client.Received(1).Post<inputMessageName1, outputMessageName1>(
            "TimeManagerService",
            "getTimeFromServer",
            "session-token",
            Arg.Is<inputMessageName1>(input => input.getTimeFromServer1 == "time.example.com"));
    }

    [TestMethod]
    public async Task GetTimeFromServer_WhenNoServerIsSpecified_UsesConfiguredServer()
    {
        ConfigureSettings(CreateSettings(serverName: "  configured.example.com  "));
        client.Post<inputMessageName1, outputMessageName1>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName1>())
            .Returns(Task.FromResult(new outputMessageName1(new WSTimeServerConnectionResult
            {
                connectionFailedDueToUnknownHost = true
            })));

        var result = await service.GetTimeFromServer();

        Assert.IsTrue(result.ConnectionFailedDueToUnknownHost);
        Assert.IsNull(result.Milliseconds);
        Assert.IsNull(result.Time);
        await client.Received(1).Post<inputMessageName1, outputMessageName1>(
            "TimeManagerService",
            "getTimeFromServer",
            "session-token",
            Arg.Is<inputMessageName1>(input => input.getTimeFromServer1 == "configured.example.com"));
    }

    [TestMethod]
    public async Task GetTimeFromServer_WhenControllerReturnsNoResult_ThrowsEmptyResponseException()
    {
        client.Post<inputMessageName1, outputMessageName1>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName1>())
            .Returns(Task.FromResult(new outputMessageName1 { getTimeFromServer2 = null! }));

        await Assert.ThrowsExactlyAsync<EmptyResponseException>(
            () => service.GetTimeFromServer("time.example.com"));
    }

    [TestMethod]
    public async Task UpdateSettings_PreservesOmittedSettingsAndConvertsManualTimeToUtc()
    {
        ConfigureSettings(CreateSettings(
            serverName: "existing.example.com",
            synchronize: false,
            interval: 8,
            gmtOffset: -4,
            useDst: true));
        client.Post<inputMessageName4, outputMessageName4>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName4>())
            .Returns(Task.FromResult(new outputMessageName4(true)));
        var currentTime = new DateTimeOffset(2026, 10, 1, 12, 34, 56, TimeSpan.FromHours(2));

        await service.UpdateSettings(new UpdateTimeSettingsRequest { CurrentTime = currentTime });

        await client.Received(1).Post<inputMessageName4, outputMessageName4>(
            "TimeManagerService",
            "setSettings",
            "session-token",
            Arg.Is<inputMessageName4>(input =>
                input.setSettings1.serverName == "existing.example.com" &&
                !input.setSettings1.synchroniseTimeAgainstServer &&
                input.setSettings1.syncIntervalInHours == 8 &&
                input.setSettings1.gmtOffsetInHours == -4 &&
                input.setSettings1.useDST &&
                input.setSettings1.timeAndDateInUTC.year == 2026 &&
                input.setSettings1.timeAndDateInUTC.monthWithJanuaryAsOne == 10 &&
                input.setSettings1.timeAndDateInUTC.day == 1 &&
                input.setSettings1.timeAndDateInUTC.hours == 10 &&
                input.setSettings1.timeAndDateInUTC.minutes == 34 &&
                input.setSettings1.timeAndDateInUTC.seconds == 56));
    }

    [TestMethod]
    public async Task UpdateSettings_WhenSynchronizationEnabledTestsServerBeforeSaving()
    {
        ConfigureSettings(CreateSettings(serverName: "time.example.com", synchronize: false));
        client.Post<inputMessageName1, outputMessageName1>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName1>())
            .Returns(Task.FromResult(new outputMessageName1(new WSTimeServerConnectionResult
            {
                connectionWasSuccessful = true
            })));
        client.Post<inputMessageName4, outputMessageName4>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName4>())
            .Returns(Task.FromResult(new outputMessageName4(true)));

        await service.UpdateSettings(new UpdateTimeSettingsRequest { Synchronize = true });

        await client.Received(1).Post<inputMessageName1, outputMessageName1>(
            "TimeManagerService",
            "getTimeFromServer",
            "session-token",
            Arg.Is<inputMessageName1>(input => input.getTimeFromServer1 == "time.example.com"));
        await client.Received(1).Post<inputMessageName4, outputMessageName4>(
            "TimeManagerService",
            "setSettings",
            "session-token",
            Arg.Is<inputMessageName4>(input =>
                input.setSettings1.synchroniseTimeAgainstServer &&
                input.setSettings1.timeAndDateInUTC == null));
    }

    [TestMethod]
    public async Task UpdateSettings_WhenSynchronizationServerIsUnreachable_ThrowsWithoutSaving()
    {
        ConfigureSettings(CreateSettings(serverName: "offline.example.com", synchronize: false));
        client.Post<inputMessageName1, outputMessageName1>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName1>())
            .Returns(Task.FromResult(new outputMessageName1(new WSTimeServerConnectionResult
            {
                connectionFailedDueToOtherErrors = true
            })));

        await Assert.ThrowsExactlyAsync<ArgumentException>(
            () => service.UpdateSettings(new UpdateTimeSettingsRequest { Synchronize = true }));

        await client.DidNotReceive().Post<inputMessageName4, outputMessageName4>(
            Arg.Any<string>(),
            "setSettings",
            Arg.Any<string>(),
            Arg.Any<inputMessageName4>());
    }

    [TestMethod]
    public async Task UpdateSettings_WhenControllerRejectsSettings_ThrowsArgumentException()
    {
        ConfigureSettings(CreateSettings(synchronize: false));
        client.Post<inputMessageName4, outputMessageName4>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName4>())
            .Returns(Task.FromResult(new outputMessageName4(false)));

        await Assert.ThrowsExactlyAsync<ArgumentException>(
            () => service.UpdateSettings(new UpdateTimeSettingsRequest { GmtOffset = 1 }));
    }

    private void ConfigureSettings(WSTimeManagerSettings settings)
    {
        client.Post<inputMessageName3, outputMessageName3>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName3>())
            .Returns(Task.FromResult(new outputMessageName3(settings)));
    }

    private static WSTimeManagerSettings CreateSettings(
        string serverName = "time.example.com",
        bool synchronize = true,
        int interval = 12,
        int gmtOffset = 0,
        bool useDst = false)
    {
        return new WSTimeManagerSettings
        {
            serverName = serverName,
            synchroniseTimeAgainstServer = synchronize,
            syncIntervalInHours = interval,
            gmtOffsetInHours = gmtOffset,
            useDST = useDst,
            timeAndDateInUTC = new WSDate()
        };
    }
}
