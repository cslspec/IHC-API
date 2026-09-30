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
    /// Update time settings.
    /// </summary>
    /// <remarks>
    /// Updates the time settings of the IHC controller. Properties that are left out keep their current value.
    /// When synchronization is enabled, the time server is tested first and the settings are only saved if it responds.
    /// The current time is only applied when the controller does not synchronize with a time server.
    /// </remarks>
    /// <param name="settings">The changes to apply.</param>
    /// <returns>An empty response when the settings were updated.</returns>
    /// <response code="204">If the time settings were updated successfully.</response>
    /// <response code="400">If the time server cannot be reached or the controller rejects the settings.</response>
    /// <response code="500">If there is an error updating the time settings.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpPost("settings")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSettings(UpdateTimeSettingsRequest settings)
    {
        await timeService.UpdateSettings(settings);
        return NoContent();
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
