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
[ApiController]
[Route("api/config")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
public class ConfigController(
    IConfigurationService configService,
    IAccessControlService accessService,
    IAuthCacheService authCacheService
) : ControllerBase
{
    /// <summary>
    /// Get system information.
    /// </summary>
    /// <remarks>
    /// Retrieves system information from the IHC controller.
    /// </remarks>
    /// <returns>The system information.</returns>
    /// <response code="200">Returns the system information.</response>
    /// <response code="500">If there is an error retrieving the system information.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("system")]
    [ProducesResponseType<SystemInfo>(StatusCodes.Status200OK)]
    public async Task<ActionResult<SystemInfo>> GetSystemInfo()
    {
        return await configService.GetSystemInfo();
    }

    /// <summary>
    /// Get network settings.
    /// </summary>
    /// <remarks>
    /// Retrieves network settings from the IHC controller.
    /// </remarks>
    /// <returns>The network settings.</returns>
    /// <response code="200">Returns the network settings.</response>
    /// <response code="500">If there is an error retrieving the network settings.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("network")]
    [ProducesResponseType<NetworkSetting>(StatusCodes.Status200OK)]
    public async Task<ActionResult<NetworkSetting>> GetNetworkSetting()
    {
        return await configService.GetNetworkSetting();
    }

    /// <summary>
    /// Get DNS servers.
    /// </summary>
    /// <remarks>
    /// Retrieves DNS servers from the IHC controller.
    /// </remarks>
    /// <returns>The DNS servers.</returns>
    /// <response code="200">Returns the DNS servers.</response>
    /// <response code="500">If there is an error retrieving the DNS servers.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("dns")]
    [ProducesResponseType<string[]>(StatusCodes.Status200OK)]
    public async Task<ActionResult<string[]>> GetDnsServers()
    {
        return await configService.GetDnsServers();
    }

    /// <summary>
    /// Get SMTP settings.
    /// </summary>
    /// <remarks>
    /// Retrieves SMTP settings from the IHC controller.
    /// </remarks>
    /// <returns>The SMTP settings.</returns>
    /// <response code="200">Returns the SMTP settings.</response>
    /// <response code="500">If there is an error retrieving the SMTP settings.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("smtp")]
    [ProducesResponseType<SmtpSettings>(StatusCodes.Status200OK)]
    public async Task<ActionResult<SmtpSettings>> GetSmtpSettings()
    {
        return await configService.GetSmtpSettings();
    }

    /// <summary>
    /// Update SMTP settings.
    /// </summary>
    /// <remarks>
    /// Updates SMTP settings in the IHC controller.
    /// </remarks>
    /// <param name="settings">The new SMTP settings to apply.</param>
    /// <returns>An empty response when the settings were updated.</returns>
    /// <response code="204">If the SMTP settings were updated successfully.</response>
    /// <response code="400">If the SMTP settings are invalid.</response>
    /// <response code="500">If there is an error updating the SMTP settings.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpPost("smtp")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSmtpSettings(SmtpSettings settings)
    {
        await configService.UpdateSmtpSettings(settings);
        return NoContent();
    }

    /// <summary>
    /// Get email settings.
    /// </summary>
    /// <remarks>
    /// Retrieves email settings from the IHC controller.
    /// </remarks>
    /// <returns>The email settings.</returns>
    /// <response code="200">Returns the email settings.</response>
    /// <response code="500">If there is an error retrieving the email settings.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("email")]
    [ProducesResponseType<EmailSettings>(StatusCodes.Status200OK)]
    public async Task<ActionResult<EmailSettings>> GetEmailSettings()
    {
        return await configService.GetEmailSettings();
    }

    /// <summary>
    /// Get email enable settings.
    /// </summary>
    /// <remarks>
    /// Retrieves email enable settings from the IHC controller.
    /// </remarks>
    /// <returns>The email enable settings.</returns>
    /// <response code="200">Returns the email enable settings.</response>
    /// <response code="500">If there is an error retrieving the email enable settings.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("email/enable")]
    [ProducesResponseType<EmailEnabledSetting>(StatusCodes.Status200OK)]
    public async Task<ActionResult<EmailEnabledSetting>> GetEmailEnableSettings()
    {
        return new EmailEnabledSetting { IsEmailEnabled = await configService.GetEmailEnableSettings() };
    }

    /// <summary>
    /// Get access control settings.
    /// </summary>
    /// <remarks>
    /// Retrieves access control settings from the IHC controller.
    /// </remarks>
    /// <returns>The access control settings.</returns>
    /// <response code="200">Returns the access control settings.</response>
    /// <response code="500">If there is an error retrieving the access control settings.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpGet("access")]
    [ProducesResponseType<AccessControlSetting[]>(StatusCodes.Status200OK)]
    public async Task<ActionResult<AccessControlSetting[]>> GetAccessControl()
    {
        return await accessService.GetAccessControl();
    }

    /// <summary>
    /// Logout.
    /// </summary>
    /// <remarks>
    /// Logs out from the IHC controller and purges the authentication cache.
    /// </remarks>
    /// <returns>An empty response when the logout was successful.</returns>
    /// <response code="204">If the logout was successful.</response>
    /// <response code="500">If there is an error during the logout process.</response>
    /// <response code="503">If there is a problem connecting to the IHC controller.</response>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Logout()
    {
        authCacheService.ClearCache();
        return NoContent();
    }
}
