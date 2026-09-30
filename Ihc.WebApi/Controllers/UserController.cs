using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ihc.WebApi.Controllers;

/// <summary>
/// Provides endpoints for user-related operations via the IHC UserManagerService API.
/// </summary>
/// <param name="userService">Service for retrieving and managing users on the controller.</param>
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

    /// <summary>
    /// Add user.
    /// </summary>
    /// <remarks>
    /// Creates a new user on the IHC controller.
    /// </remarks>
    /// <param name="user">The user to create.</param>
    /// <returns>An empty response when the user was created.</returns>
    /// <response code="204">If the user was created successfully.</response>
    /// <response code="400">If the user is invalid or a user with the same username already exists.</response>
    /// <response code="500">If there is an error creating the user.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUser(AddUserRequest user)
    {
        await userService.AddUser(user);
        return NoContent();
    }

    /// <summary>
    /// Update user.
    /// </summary>
    /// <remarks>
    /// Updates an existing user on the IHC controller. Properties that are left out keep their current value.
    /// The username cannot be changed.
    /// </remarks>
    /// <param name="username">The username of the user to update.</param>
    /// <param name="update">The changes to apply.</param>
    /// <returns>An empty response when the user was updated.</returns>
    /// <response code="204">If the user was updated successfully.</response>
    /// <response code="400">If the changes are invalid.</response>
    /// <response code="404">If no user with the specified username exists.</response>
    /// <response code="500">If there is an error updating the user.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpPut("{username}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(string username, UpdateUserRequest update)
    {
        await userService.UpdateUser(username, update);
        return NoContent();
    }

    /// <summary>
    /// Remove user.
    /// </summary>
    /// <remarks>
    /// Removes a user from the IHC controller. The administrator and the user this API logs in with cannot be removed.
    /// </remarks>
    /// <param name="username">The username of the user to remove.</param>
    /// <returns>An empty response when the user was removed.</returns>
    /// <response code="204">If the user was removed successfully.</response>
    /// <response code="400">If the user is the administrator or the user this API logs in with.</response>
    /// <response code="404">If no user with the specified username exists.</response>
    /// <response code="500">If there is an error removing the user.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpDelete("{username}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveUser(string username)
    {
        await userService.RemoveUser(username);
        return NoContent();
    }
}
