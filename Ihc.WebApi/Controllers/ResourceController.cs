using Ihc.WebApi.Model.Resource;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ihc.WebApi.Controllers;

/// <summary>
/// Provides endpoints for resource values via the IHC ResourceInteractionService API.
/// </summary>
/// <param name="resourceService">Service for reading resource values from the controller.</param>
[ApiController]
[Route("api/resources")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
public class ResourceController(IResourceService resourceService) : ControllerBase
{
    /// <summary>
    /// Get runtime value.
    /// </summary>
    /// <remarks>
    /// Retrieves the current runtime value of a resource from the IHC controller.
    /// Resource identifiers can be found in the project model.
    /// </remarks>
    /// <param name="resourceId">The resource identifier.</param>
    /// <returns>The runtime value of the resource.</returns>
    /// <response code="200">Returns the runtime value.</response>
    /// <response code="404">If the controller has no value for the resource.</response>
    /// <response code="500">If there is an error retrieving the runtime value.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("{resourceId:int}/runtime")]
    [ProducesResponseType<ResourceValue>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ResourceValue>> GetRuntimeValue(int resourceId)
    {
        return await resourceService.GetRuntimeValue(resourceId);
    }
}
