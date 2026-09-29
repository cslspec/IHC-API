using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using ProjectModel = Ihc.Project.Model.Project;

namespace Ihc.WebApi.Controllers;

/// <summary>
/// Provides endpoints for project-related operations via the IHC ControllerService API.
/// </summary>
/// <param name="projectService">Service for retrieving project data from the controller.</param>
[ApiController]
[Route("api/project")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
public class ProjectController(IProjectService projectService) : ControllerBase
{
    /// <summary>
    /// Check project availability.
    /// </summary>
    /// <remarks>
    /// Retrieves the project availability from the IHC controller.
    /// </remarks>
    /// <returns>The project availability.</returns>
    /// <response code="200">Returns whether a project is available.</response>
    /// <response code="500">If there is an error retrieving project availability.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("available")]
    [ProducesResponseType<ProjectAvailability>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProjectAvailability>> GetIsProjectAvailable()
    {
        return new ProjectAvailability { IsProjectAvailable = await projectService.GetIsProjectAvailable() };
    }

    /// <summary>
    /// Get project information.
    /// </summary>
    /// <remarks>
    /// Retrieves the project information from the IHC controller.
    /// </remarks>
    /// <returns>The project information.</returns>
    /// <response code="200">Returns the project information.</response>
    /// <response code="500">If there is an error retrieving the project information.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("info")]
    [ProducesResponseType<ProjectInfo>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProjectInfo>> GetProjectInfo()
    {
        return await projectService.GetProjectInfo();
    }

    /// <summary>
    /// Get project file.
    /// </summary>
    /// <remarks>
    /// Retrieves the project file from the IHC controller.
    /// </remarks>
    /// <returns>The project file.</returns>
    /// <response code="200">Returns the project file.</response>
    /// <response code="500">If there is an error retrieving the project file.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("file")]
    [ProducesResponseType<ProjectFile>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProjectFile>> GetProjectFile()
    {
        var content = await projectService.GetProjectFile();
        return new ProjectFile { Size = content.Length, Content = content };
    }

    /// <summary>
    /// Get project model.
    /// </summary>
    /// <remarks>
    /// Retrieves the project model from the IHC controller.
    /// </remarks>
    /// <returns>The project model.</returns>
    /// <response code="200">Returns the project model.</response>
    /// <response code="500">If there is an error retrieving the project model.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("model")]
    [ProducesResponseType<ProjectModel>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProjectModel>> GetModel()
    {
        return new ProjectModel(await projectService.GetProjectFile());
    }
}
