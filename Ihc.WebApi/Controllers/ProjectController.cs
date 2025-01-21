using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ihc.WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProjectController(IProjectService projectService) : ControllerBase
    {
        [HttpGet]
        [Route("project/available")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetIsProjectAvailable()
        {
            try
            {
                var info = await projectService.GetIsProjectAvailable();
                return Ok(info);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("project/info")]
        [ProducesResponseType<ProjectInfo>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProjectInfo()
        {
            try
            {
                var info = await projectService.GetProjectInfo();
                return Ok(info);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("project/file")]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProjectFile()
        {
            try
            {
                var info = await projectService.GetProjectFile();
                return Ok(info);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("project/model")]
        [ProducesResponseType<Project.Model.Project>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetModel()
        {
            try
            {
                var info = await projectService.GetProjectFile();
                var project = new Project.Model.Project(info);
                return Ok(project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
