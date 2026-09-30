using Ihc.Soap.TimeManager;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model.Time;

namespace Ihc.WebApi.Services;

/// <summary>
/// Retrieves and updates time settings on the IHC controller.
/// </summary>
public interface ITimeService
{
    /// <summary>Retrieves the controller's uptime.</summary>
    /// <returns>The uptime broken down into time components.</returns>
    Task<Uptime> GetUptime();

    /// <summary>Retrieves the controller's current local time.</summary>
    /// <returns>The current local date and time.</returns>
    /// <exception cref="EmptyResponseException">The controller returns no time value.</exception>
    Task<DateTime> GetLocalTime();

    /// <summary>Retrieves the configured time settings.</summary>
    /// <returns>The current time settings.</returns>
    /// <exception cref="EmptyResponseException">The controller returns no settings.</exception>
    Task<TimeSettings> GetSettings();

    /// <summary>Queries a time server for its current time.</summary>
    /// <param name="serverName">The time server to query, or <see langword="null"/> to query the configured time server.</param>
    /// <returns>The connection result and, when successful, the server time.</returns>
    /// <exception cref="EmptyResponseException">The controller returns no connection result.</exception>
    Task<TimeServerConnectionResult> GetTimeFromServer(string? serverName = null);

    /// <summary>Updates the controller's time settings. Values that are not specified keep their current value.</summary>
    /// <param name="settings">The changes to apply.</param>
    /// <exception cref="ArgumentException">The time server cannot be reached or the controller rejects the settings.</exception>
    /// <exception cref="EmptyResponseException">The controller returns no update result.</exception>
    Task UpdateSettings(UpdateTimeSettingsRequest settings);
}

/// <summary>
/// Provides functionality to interact with the TimeManagerService, handling operations 
/// such as fetching uptime, current time, and managing time settings.
/// </summary>
/// <param name="client">The SOAP client used to communicate with the controller.</param>
/// <param name="dateService">Service for converting SOAP dates.</param>
/// <param name="authCache">Service for obtaining the current authentication token.</param>
public class TimeService(
    IClientService client,
    ISoapDateService dateService,
    IAuthCacheService authCache
) : ITimeService
{
    private const string ServiceName = "TimeManagerService";

    /// <summary>
    /// Retrieves the system's uptime from the IHC TimeManagerService.
    /// </summary>
    /// <returns>An <see cref="Uptime"/> object representing the system's uptime.</returns>
    /// <exception cref="EmptyResponseException">Thrown if the response is null or contains no data.</exception>
    public async Task<Uptime> GetUptime()
    {
        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName5, outputMessageName5>(
            ServiceName, "getUptime", token!, new inputMessageName5());

        if (response?.getUptime1 == null)
        {
            throw new EmptyResponseException();
        }

        var timeSpan = response.getUptime1.HasValue
            ? TimeSpan.FromMilliseconds(response.getUptime1.Value)
            : TimeSpan.Zero;

        var result = new Uptime
        {
            Time = timeSpan,
            Days = timeSpan.Days,
            Hours = timeSpan.Hours,
            Minutes = timeSpan.Minutes,
            Seconds = timeSpan.Seconds,
            Milliseconds = timeSpan.Milliseconds,
            TotalMilliseconds = response.getUptime1.Value
        };

        return result;
    }

    /// <summary>
    /// Retrieves the current local time from the IHC TimeManagerService.
    /// </summary>
    /// <returns>The local time as a <see cref="DateTime"/> object.</returns>
    /// <exception cref="EmptyResponseException">Thrown if the response is null or contains no data.</exception>
    public async Task<DateTime> GetLocalTime()
    {
        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName2, outputMessageName2>(
            ServiceName, "getCurrentLocalTime", token!, new inputMessageName2());

        if (response?.getCurrentLocalTime1 == null)
        {
            throw new EmptyResponseException();
        }

        var result = dateService.GetDateTime(response.getCurrentLocalTime1);
        return result;
    }

    /// <summary>
    /// Retrieves the current time settings from the IHC TimeManagerService.
    /// </summary>
    /// <returns>A <see cref="TimeSettings"/> object representing the configuration.</returns>
    /// <exception cref="EmptyResponseException">Thrown if the response is null or contains no data.</exception>
    public async Task<TimeSettings> GetSettings()
    {
        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName3, outputMessageName3>(
            ServiceName, "getSettings", token!, new inputMessageName3());

        if (response?.getSettings1 == null)
        {
            throw new EmptyResponseException();
        }

        var info = response.getSettings1;
        var result = new TimeSettings
        {
            TimeServerName = info.serverName,
            Synchronize = info.synchroniseTimeAgainstServer,
            SynchronizeInterval = info.syncIntervalInHours,
            GmtOffset = info.gmtOffsetInHours,
            UseDst = info.useDST,
            CurrentTime = dateService.GetDateTime(info.timeAndDateInUTC, DateTimeKind.Utc)
        };

        return result;
    }

    /// <summary>
    /// Retrieves the current time from a time server through the IHC controller.
    /// </summary>
    /// <param name="serverName">The time server to query, or <see langword="null"/> to query the configured time server.</param>
    /// <returns>A <see cref="TimeServerConnectionResult"/> object containing connection details and time.</returns>
    /// <exception cref="EmptyResponseException">Thrown if the response is null or contains no data.</exception>
    public async Task<TimeServerConnectionResult> GetTimeFromServer(string? serverName = null)
    {
        // The controller does not fall back to the configured server, so a server name is always sent.
        serverName = serverName?.Trim();
        if (string.IsNullOrEmpty(serverName))
        {
            serverName = (await GetSettings()).TimeServerName?.Trim();
        }

        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName1, outputMessageName1>(
            ServiceName, "getTimeFromServer", token!, new inputMessageName1(serverName));

        if (response?.getTimeFromServer2 == null)
        {
            throw new EmptyResponseException();
        }

        var info = response.getTimeFromServer2;

        // The controller reports the server time in milliseconds since the Unix epoch.
        var result = new TimeServerConnectionResult
        {
            ConnectionWasSuccessful = info.connectionWasSuccessful,
            ConnectionFailedDueToUnknownHost = info.connectionFailedDueToUnknownHost,
            ConnectionFailedDueToOtherErrors = info.connectionFailedDueToOtherErrors,
            Milliseconds = info.connectionWasSuccessful ? info.dateFromServer : null,
            Time = info.connectionWasSuccessful
                ? DateTimeOffset.FromUnixTimeMilliseconds(info.dateFromServer).UtcDateTime
                : null
        };

        return result;
    }

    /// <summary>
    /// Updates the time settings in the IHC TimeManagerService.
    /// </summary>
    /// <remarks>
    /// The controller replaces all settings, so values that are not specified are copied from the current settings.
    /// The time server is tested before synchronization is enabled, and a manual time is only sent when the
    /// controller does not synchronize with a time server.
    /// </remarks>
    /// <param name="settings">The changes to apply.</param>
    /// <exception cref="ArgumentException">The time server cannot be reached or the controller rejects the settings.</exception>
    /// <exception cref="EmptyResponseException">Thrown if the response is null or contains no data.</exception>
    public async Task UpdateSettings(UpdateTimeSettingsRequest settings)
    {
        var current = await GetSettings();

        var serverName = settings.TimeServerName ?? current.TimeServerName ?? string.Empty;
        var synchronize = settings.Synchronize ?? current.Synchronize ?? false;

        if (synchronize)
        {
            var test = await GetTimeFromServer(serverName);
            if (!test.ConnectionWasSuccessful)
            {
                throw new ArgumentException($"The time server '{serverName}' could not be reached.", nameof(settings));
            }
        }

        var input = new inputMessageName4
        {
            setSettings1 = new WSTimeManagerSettings
            {
                serverName = serverName,
                synchroniseTimeAgainstServer = synchronize,
                syncIntervalInHours = settings.SynchronizeInterval ?? current.SynchronizeInterval ?? 0,
                useDST = settings.UseDst ?? current.UseDst ?? false,
                gmtOffsetInHours = settings.GmtOffset ?? current.GmtOffset ?? 0
            }
        };

        // Only set the clock when it is not synchronized with a time server.
        if (!synchronize && settings.CurrentTime != null)
        {
            var utc = settings.CurrentTime.Value.UtcDateTime;
            input.setSettings1.timeAndDateInUTC = new WSDate
            {
                year = utc.Year,
                monthWithJanuaryAsOne = utc.Month,
                day = utc.Day,
                hours = utc.Hour,
                minutes = utc.Minute,
                seconds = utc.Second
            };
        }

        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName4, outputMessageName4>(
            ServiceName, "setSettings", token!, input);

        if (response?.setSettings2 == null)
        {
            throw new EmptyResponseException();
        }

        if (!response.setSettings2.Value)
        {
            throw new ArgumentException("The controller rejected the time settings.", nameof(settings));
        }
    }
}
