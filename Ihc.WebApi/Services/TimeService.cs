using Ihc.Soap.TimeManager;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;

namespace Ihc.WebApi.Services
{
    public interface ITimeService
    {
        Task<Uptime> GetUptime();

        Task<DateTime> GetLocalTime();

        Task<TimeSettings> GetSettings();

        Task<TimeServerConnectionResult> GetTimeFromServer();

        Task<bool?> UpdateSettings(TimeSettings settings);
    }

    /// <summary>
    /// Provides functionality to interact with the TimeManagerService, handling operations 
    /// such as fetching uptime, current time, and managing time settings.
    /// </summary>
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
                throw new EmptyResponseException();

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
                throw new EmptyResponseException();

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
                throw new EmptyResponseException();

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
        /// Retrieves the current time on the IHC system directly from the time configured server server.
        /// </summary>
        /// <returns>A <see cref="TimeServerConnectionResult"/> object containing connection details and time.</returns>
        /// <exception cref="EmptyResponseException">Thrown if the response is null or contains no data.</exception>
        public async Task<TimeServerConnectionResult> GetTimeFromServer()
        {
            var token = authCache.GetAuthToken().Token;
            var response = await client.Post<inputMessageName1, outputMessageName1>(
                ServiceName, "getTimeFromServer", token!, new inputMessageName1());

            if (response?.getTimeFromServer2 == null)
                throw new EmptyResponseException();

            var info = response.getTimeFromServer2;

            var baseline = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
            var time = baseline.AddSeconds(info.dateFromServer);

            var result = new TimeServerConnectionResult
            {
                ConnectionWasSuccessful = info.connectionWasSuccessful,
                ConnectionFailedDueToUnknownHost = info.connectionFailedDueToUnknownHost,
                ConnectionFailedDueToOtherErrors = info.connectionFailedDueToOtherErrors,
                Seconds = info.connectionWasSuccessful ? info.dateFromServer : null,
                Time = info.connectionWasSuccessful ? time : null
            };

            return result;
        }

        /// <summary>
        /// Updates the time server settings in the IHC TimeManagerService.
        /// </summary>
        /// <param name="settings">The new time settings to apply.</param>
        /// <returns>A boolean indicating whether the settings were successfully updated.</returns>
        /// <exception cref="EmptyResponseException">Thrown if the response is null or contains no data.</exception>
        public async Task<bool?> UpdateSettings(TimeSettings settings)
        {
            var token = authCache.GetAuthToken().Token;

            var input = new inputMessageName4
            {
                setSettings1 = new WSTimeManagerSettings()
            };

            if (settings.TimeServerName !=  null)
            {
                input.setSettings1.serverName = settings.TimeServerName;
            }

            if (settings.Synchronize != null)
            {
                input.setSettings1.synchroniseTimeAgainstServer = settings.Synchronize.Value;
            }

            if (settings.SynchronizeInterval != null)
            {
                input.setSettings1.syncIntervalInHours = settings.SynchronizeInterval.Value;
            }

            if (settings.UseDst != null)
            {
                input.setSettings1.useDST = settings.UseDst.Value;
            }

            if (settings.GmtOffset != null)
            {
                input.setSettings1.gmtOffsetInHours = settings.GmtOffset.Value;
            }

            if (settings.CurrentTime != null)
            {
                input.setSettings1.timeAndDateInUTC = new WSDate
                {
                    year = settings.CurrentTime.Value.Year,
                    monthWithJanuaryAsOne = settings.CurrentTime.Value.Month,
                    day = settings.CurrentTime.Value.Day,
                    hours = settings.CurrentTime.Value.Hour,
                    minutes = settings.CurrentTime.Value.Minute,
                    seconds = settings.CurrentTime.Value.Second                    
                };
            }

            var response = await client.Post<inputMessageName4, outputMessageName4>(
                ServiceName, "setSettings", token!, input);

            if (response?.setSettings2 == null)
                throw new EmptyResponseException();

            var result = response.setSettings2.Value;
            return result;
        }
    }
}
