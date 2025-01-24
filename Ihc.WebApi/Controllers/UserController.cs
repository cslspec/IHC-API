using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ihc.WebApi.Controllers
{
    /// <summary>
    /// Manage user-related operations via the IHC UserManagerService API.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class UserController(
        IUserService userService,
        IProblemService problemService
        ) : ControllerBase
    {
        /// <summary>
        /// Retrieves a list of users from the IHC controller.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of users or an error response.</returns>  
        /// <response code="200">Returns the list of users.</response>  
        /// <response code="500">If there is an error retrieving the users.</response>  
        /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
        [HttpGet]
        [Route("user")]
        [ProducesResponseType<IhcUser[]>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var info = await userService.GetUsers(false);
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
