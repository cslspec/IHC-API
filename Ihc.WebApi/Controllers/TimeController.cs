using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ihc.WebApi.Controllers
{
    /// <summary>
    /// Manage time-related operations via the IHC TimeManagerService API.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class TimeController(
        ITimeService timeService,
        IProblemService problemService
        ) : ControllerBase
    {
        /// <summary>
        /// Retrieves the system's uptime.
        /// </summary>
        /// <returns>An <see cref="Uptime"/> object representing the system's uptime.</returns>
        /// <response code="200">Returns the system uptime.</response>
        /// <response code="500">If there is an error retrieving the uptime.</response>
        /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
        [HttpGet]
        [Route("time/uptime")]
        [ProducesResponseType<Uptime>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetUptime()
        {
            try
            {
                var info = await timeService.GetUptime();
                return Ok(info);
            }
            catch (Exception e)
            {
                var problem = problemService.GetProblemDetails(e);
                return StatusCode(problem.Status ?? 418, problem);
            }
        }

        /// <summary>
        /// Retrieves the current local time.
        /// </summary>
        /// <returns>The current local time as a <see cref="DateTime"/>.</returns>
        /// <response code="200">Returns the current local time.</response>
        /// <response code="500">If there is an error retrieving the local time.</response>
        /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
        [HttpGet]
        [Route("time/localtime")]
        [ProducesResponseType<DateTime>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetLocalTime()
        {
            try
            {
                var info = await timeService.GetLocalTime();
                return Ok(info);
            }
            catch (Exception e)
            {
                var problem = problemService.GetProblemDetails(e);
                return StatusCode(problem.Status ?? 418, problem);
            }
        }

        /// <summary>
        /// Retrieves the current time settings.
        /// </summary>
        /// <returns>A <see cref="TimeSettings"/> object representing the current configuration.</returns>
        /// <response code="200">Returns the current time settings.</response>
        /// <response code="500">If there is an error retrieving the time settings.</response>
        /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
        [HttpGet]
        [Route("time/settings")]
        [ProducesResponseType<TimeSettings>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetSettings()
        {
            try
            {
                var info = await timeService.GetSettings();
                return Ok(info);
            }
            catch (Exception e)
            {
                var problem = problemService.GetProblemDetails(e);
                return StatusCode(problem.Status ?? 418, problem);
            }
        }

        /// <summary>
        /// Retrieves the current time on the IHC system directly from the time configured server.
        /// </summary>
        /// <returns>A <see cref="TimeServerConnectionResult"/> object containing connection details and time.</returns>
        /// <response code="200">Returns the server time and connection details.</response>
        /// <response code="500">If there is an error retrieving the server time.</response>
        /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
        [HttpGet]
        [Route("time/settings/test")]
        [ProducesResponseType<TimeServerConnectionResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetTimeFromServer()
        {
            try
            {
                var info = await timeService.GetTimeFromServer();
                return Ok(info);
            }
            catch (Exception e)
            {
                var problem = problemService.GetProblemDetails(e);
                return StatusCode(problem.Status ?? 418, problem);
            }
        }
    }
}
