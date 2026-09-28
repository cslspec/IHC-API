using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ihc.WebApi.Controllers;

/// <summary>  
/// Provides endpoints for retrieving and updating various configuration settings.  
/// </summary>  
/// <param name="configService">Service for retrieving and updating controller configuration.</param>
/// <param name="accessService">Service for retrieving access control settings.</param>
/// <param name="authCacheService">Service for managing the authenticated controller session.</param>
/// <param name="problemService">Service for creating API problem details.</param>
[ApiController]
[Route("api")]
public class ConfigController(
       IConfigurationService configService,
       IAccessControlService accessService,
       IAuthCacheService authCacheService,
       IProblemService problemService
   ) : ControllerBase
{
    /// <summary>  
    /// Executes a service call and handles exceptions by returning appropriate HTTP status codes.  
    /// </summary>  
    /// <typeparam name="T">The type of the result returned by the service call.</typeparam>  
    /// <param name="serviceCall">The service call to execute.</param>  
    /// <returns>An <see cref="IActionResult"/> containing the result or an error response.</returns>  
    private async Task<IActionResult> ExecuteServiceCall<T>(Func<Task<T>> serviceCall)
    {
        try
        {
            var result = await serviceCall();
            return Ok(result);
        }
        catch (Exception e)
        {
            var problem = problemService.GetProblemDetails(e);
            return StatusCode(problem.Status ?? 500, problem);
        }
    }

    /// <summary>  
    /// Gets system information from the IHC controller.  
    /// </summary>  
    /// <returns>An <see cref="IActionResult"/> containing the system information or an error response.</returns>  
    /// <response code="200">Returns the system information.</response>  
    /// <response code="500">If there is an error retrieving the system information.</response>  
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
    [HttpGet]
    [Route("config/system")]
    [ProducesResponseType<SystemInfo>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetSystemInfo()
    {
        return await ExecuteServiceCall(configService.GetSystemInfo);
    }

    /// <summary>  
    /// Gets network settings from the IHC controller.  
    /// </summary>  
    /// <returns>An <see cref="IActionResult"/> containing the network settings or an error response.</returns>  
    /// <response code="200">Returns the network settings.</response>  
    /// <response code="500">If there is an error retrieving the network settings.</response>  
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
    [HttpGet]
    [Route("config/network")]
    [ProducesResponseType<NetworkSetting>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetNetworkSetting()
    {
        return await ExecuteServiceCall(configService.GetNetworkSetting);
    }

    /// <summary>  
    /// Gets DNS servers from the IHC controller.  
    /// </summary>  
    /// <returns>An <see cref="IActionResult"/> containing the DNS servers or an error response.</returns>  
    /// <response code="200">Returns the DNS servers.</response>  
    /// <response code="500">If there is an error retrieving the DNS servers.</response>  
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
    [HttpGet]
    [Route("config/dns")]
    [ProducesResponseType<string[]>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetDnsServers()
    {
        return await ExecuteServiceCall(configService.GetDnsServers);
    }

    /// <summary>  
    /// Gets SMTP settings from the IHC controller.  
    /// </summary>  
    /// <returns>An <see cref="IActionResult"/> containing the SMTP settings or an error response.</returns>  
    /// <response code="200">Returns the SMTP settings.</response>  
    /// <response code="500">If there is an error retrieving the SMTP settings.</response>  
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
    [HttpGet]
    [Route("config/smtp")]
    [ProducesResponseType<SmtpSettings>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetSmtpSettings()
    {
        return await ExecuteServiceCall(configService.GetSmtpSettings);
    }

    /// <summary>  
    /// Updates SMTP settings in the IHC controller.  
    /// </summary>  
    /// <param name="settings">The new SMTP settings to apply.</param>  
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation or an error response.</returns>  
    /// <response code="200">If the SMTP settings were updated successfully.</response>  
    /// <response code="400">If the SMTP settings could not be applied.</response>  
    /// <response code="500">If there is an error updating the SMTP settings.</response>  
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
    [HttpPost]
    [Route("config/smtp")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> UpdateSmtpSettings(SmtpSettings settings)
    {
        try
        {
            var result = await configService.UpdateSmtpSettings(settings);
            if (result == null)
            {
                return Ok();
            }

            const int failureCode = StatusCodes.Status400BadRequest;
            result.Status = failureCode;
            result.Instance = Request.Path;
            return StatusCode(failureCode, result);
        }
        catch (Exception e)
        {
            var problem = problemService.GetProblemDetails(e);
            return StatusCode(problem.Status ?? 500, problem);
        }
    }

    /// <summary>  
    /// Gets email settings from the IHC controller.  
    /// </summary>  
    /// <returns>An <see cref="IActionResult"/> containing the email settings or an error response.</returns>  
    /// <response code="200">Returns the email settings.</response>  
    /// <response code="500">If there is an error retrieving the email settings.</response>  
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
    [HttpGet]
    [Route("config/email")]
    [ProducesResponseType<EmailSettings>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetEmailSettings()
    {
        return await ExecuteServiceCall(configService.GetEmailSettings);
    }

    /// <summary>  
    /// Gets email enable settings from the IHC controller.  
    /// </summary>  
    /// <returns>An <see cref="IActionResult"/> containing the email enable settings or an error response.</returns>  
    /// <response code="200">Returns the email enable settings.</response>  
    /// <response code="500">If there is an error retrieving the email enable settings.</response>  
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
    [HttpGet]
    [Route("config/email/enable")]
    [ProducesResponseType<EmailEnabledSetting>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetEmailEnableSettings()
    {
        try
        {
            var result = new EmailEnabledSetting
            {
                IsEmailEnabled = await configService.GetEmailEnableSettings()
            };

            return Ok(result);
        }
        catch (Exception e)
        {
            var problem = problemService.GetProblemDetails(e);
            return StatusCode(problem.Status ?? 500, problem);
        }
    }

    /// <summary>  
    /// Gets access control settings from the IHC controller.  
    /// </summary>  
    /// <returns>An <see cref="IActionResult"/> containing the access control settings or an error response.</returns>  
    /// <response code="200">Returns the access control settings.</response>  
    /// <response code="500">If there is an error retrieving the access control settings.</response>  
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
    [HttpGet]
    [Route("config/access")]
    [ProducesResponseType<AccessControlSetting[]>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetAccessControl()
    {
        return await ExecuteServiceCall(accessService.GetAccessControl);
    }

    /// <summary>  
    /// Logs out from the IHC controller and purges the authentication cache.  
    /// </summary>  
    /// <returns>An <see cref="IActionResult"/> indicating the result of the logout operation or an error response.</returns>  
    /// <response code="200">If the logout was successful.</response>  
    /// <response code="500">If there is an error during the logout process.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>  
    [HttpGet]
    [Route("config/logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
    public IActionResult Logout()
    {
        try
        {
            authCacheService.ClearCache();
            return Ok();
        }
        catch (Exception e)
        {
            var problem = problemService.GetProblemDetails(e);
            return StatusCode(problem.Status ?? 500, problem);
        }
    }
}
