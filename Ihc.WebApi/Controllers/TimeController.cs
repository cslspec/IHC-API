using Ihc.WebApi.Model.Time;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ihc.WebApi.Controllers;

/// <summary>
/// Provides endpoints for time-related operations via the IHC TimeManagerService API.
/// </summary>
/// <param name="timeService">Service for retrieving and updating controller time data.</param>
[ApiController]
[Route("api/time")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
public class TimeController(ITimeService timeService) : ControllerBase
{
    /// <summary>
    /// Get system uptime.
    /// </summary>
    /// <remarks>
    /// Retrieves the system's uptime from the IHC controller.
    /// </remarks>
    /// <returns>An <see cref="Uptime"/> object representing the system's uptime.</returns>
    /// <response code="200">Returns the system uptime.</response>
    /// <response code="500">If there is an error retrieving the uptime.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("uptime")]
    [ProducesResponseType<Uptime>(StatusCodes.Status200OK)]
    public async Task<ActionResult<Uptime>> GetUptime()
    {
        return await timeService.GetUptime();
    }

    /// <summary>
    /// Get local time.
    /// </summary>
    /// <remarks>
    /// Retrieves the current local time from the IHC controller.
    /// </remarks>
    /// <returns>The current local time of the controller.</returns>
    /// <response code="200">Returns the current local time.</response>
    /// <response code="500">If there is an error retrieving the local time.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("localtime")]
    [ProducesResponseType<CurrentTime>(StatusCodes.Status200OK)]
    public async Task<ActionResult<CurrentTime>> GetLocalTime()
    {
        return new CurrentTime { CurrentLocalTime = await timeService.GetLocalTime() };
    }

    /// <summary>
    /// Get time settings.
    /// </summary>
    /// <remarks>
    /// Retrieves the current time settings from the IHC controller.
    /// </remarks>
    /// <returns>A <see cref="TimeSettings"/> object representing the current configuration.</returns>
    /// <response code="200">Returns the current time settings.</response>
    /// <response code="500">If there is an error retrieving the time settings.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("settings")]
    [ProducesResponseType<TimeSettings>(StatusCodes.Status200OK)]
    public async Task<ActionResult<TimeSettings>> GetSettings()
    {
        return await timeService.GetSettings();
    }

    /// <summary>
    /// Test time server connection.
    /// </summary>
    /// <remarks>
    /// Retrieves the current time from the configured time server through the IHC controller.
    /// </remarks>
    /// <returns>A <see cref="TimeServerConnectionResult"/> object containing connection details and time.</returns>
    /// <response code="200">Returns the server time and connection details.</response>
    /// <response code="500">If there is an error retrieving the server time.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpPost("settings/test")]
    [ProducesResponseType<TimeServerConnectionResult>(StatusCodes.Status200OK)]
    public async Task<ActionResult<TimeServerConnectionResult>> GetTimeFromServer()
    {
        return await timeService.GetTimeFromServer();
    }
}
