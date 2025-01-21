using Ihc.Project.Model;
using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ihc.WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ConfigController(
        IConfigurationService configService,
        IAccessControlService accessService,
        IAuthCacheService authCacheService)
        : ControllerBase
    {
        [HttpGet]
        [Route("config/system")]
        [ProducesResponseType<Uptime>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSystemInfo()
        {
            var info = await configService.GetSystemInfo();
            return Ok(info);
        }

        [HttpGet]
        [Route("config/network")]
        [ProducesResponseType<NetworkSetting>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNetworkSetting()
        {
            var info = await configService.GetNetworkSetting();
            return Ok(info);
        }

        [HttpGet]
        [Route("config/dns")]
        [ProducesResponseType<string[]>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDnsServers()
        {
            var info = await configService.GetDnsServers();
            return Ok(info);
        }

        [HttpGet]
        [ProducesResponseType<SmtpSettings>(StatusCodes.Status200OK)]
        [Route("config/smtp")]
        public async Task<IActionResult> GetSmtpSettings()
        {
            var info = await configService.GetSmtpSettings();
            return Ok(info);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("config/smtp")]
        public async Task<IActionResult> UpdateSmtpSettings(SmtpSettings settings)
        {
            var result = await configService.UpdateSmtpSettings(settings);
            if (result == null)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("config/email")]
        [ProducesResponseType<EmailSettings>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmailSettings()
        {
            var info = await configService.GetEmailSettings();
            return Ok(info);
        }

        [HttpGet]
        [Route("config/email/enable")]
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmailEnableSettings()
        {
            var info = await configService.GetEmailEnableSettings();
            return Ok(info);
        }

        [HttpGet]
        [Route("config/access")]
        [ProducesResponseType<AccessControlSetting[]>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccessControl()
        {
            var info = await accessService.GetAccessControl();
            return Ok(info);
        }

        [HttpGet]
        [Route("config/logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            await Task.Run(authCacheService.ClearCache);
            return Ok();
        }
    }
}
