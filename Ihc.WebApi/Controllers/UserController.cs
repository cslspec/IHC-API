using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ihc.WebApi.Controllers;

/// <summary>
/// Provides endpoints for user-related operations via the IHC UserManagerService API.
/// </summary>
/// <param name="userService">Service for retrieving users from the controller.</param>
[ApiController]
[Route("api/users")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
public class UserController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Get users.
    /// </summary>
    /// <remarks>
    /// Retrieves a list of users from the IHC controller.
    /// </remarks>
    /// <returns>The list of users.</returns>
    /// <response code="200">Returns the list of users.</response>
    /// <response code="500">If there is an error retrieving the users.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet]
    [ProducesResponseType<IhcUser[]>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IhcUser[]>> GetUsers()
    {
        return await userService.GetUsers(includePassword: false);
    }
}
