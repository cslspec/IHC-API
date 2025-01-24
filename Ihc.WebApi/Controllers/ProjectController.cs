using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ihc.WebApi.Controllers
{
    /// <summary>
    /// Manage project-related operations via the IHC ControllerService API.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class ProjectController(
        IProjectService projectService,
       IProblemService problemService
        ) : ControllerBase
    {
        /// <summary>
        /// Retrieves the project availability from the IHC controller.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the project availability or an error response.</returns>  
        /// <response code="200">Returns the system information.</response>  
        /// <response code="500">If there is an error retrieving the system information.</response>  
        /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
        [HttpGet]
        [Route("project/available")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetIsProjectAvailable()
        {
            try
            {
                var info = await projectService.GetIsProjectAvailable();
                return Ok(info);
            }
            catch (Exception e)
            {
                var problem = problemService.GetProblemDetails(e);
                return StatusCode(problem.Status ?? 418, problem);
            }
        }

        /// <summary>
        /// Retrieves the project information from the IHC controller.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the project information or an error response.</returns>  
        /// <response code="200">Returns the project information.</response>  
        /// <response code="500">If there is an error retrieving the project information.</response>  
        /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
        [HttpGet]
        [Route("project/info")]
        [ProducesResponseType<ProjectInfo>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetProjectInfo()
        {
            try
            {
                var info = await projectService.GetProjectInfo();
                return Ok(info);
            }
            catch (Exception e)
            {
                var problem = problemService.GetProblemDetails(e);
                return StatusCode(problem.Status ?? 418, problem);
            }
        }

        /// <summary>
        /// Retrieves the project file from the IHC controller.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the project file or an error response.</returns>  
        /// <response code="200">Returns the project file.</response>  
        /// <response code="500">If there is an error retrieving the project file.</response>  
        /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
        [HttpGet]
        [Route("project/file")]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetProjectFile()
        {
            try
            {
                var info = await projectService.GetProjectFile();
                return Ok(info);
            }
            catch (Exception e)
            {
                var problem = problemService.GetProblemDetails(e);
                return StatusCode(problem.Status ?? 418, problem);
            }
        }

        /// <summary>
        /// Retrieves the project model from the IHC controller.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the project model or an error response.</returns>  
        /// <response code="200">Returns the project model.</response>  
        /// <response code="500">If there is an error retrieving the project model.</response>  
        /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
        [HttpGet]
        [Route("project/model")]
        [ProducesResponseType<Project.Model.Project>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetModel()
        {
            try
            {
                var info = await projectService.GetProjectFile();
                var project = new Project.Model.Project(info);
                return Ok(project);
            }
            catch (Exception e)
            {
                var problem = problemService.GetProblemDetails(e);
                return StatusCode(problem.Status ?? 418, problem);
            }
        }
    }
}
