using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ihc.WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController(IUserService userService)
    : ControllerBase
    {
        [HttpGet]
        [Route("user")]
        [ProducesResponseType<IhcUser[]>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var info = await userService.GetUsers(false);
                return Ok(info);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
