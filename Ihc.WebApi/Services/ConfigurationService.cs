using Ihc.Soap.Configuration;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Extensions;
using Ihc.WebApi.Model;
using Ihc.WebApi.Model.Time;
using Ihc.WebApi.Util;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ihc.WebApi.Services;

/// <summary>
/// Service for handling configuration settings on the LK IHC controller.
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Gets the system information from the LK IHC controller.
    /// </summary>
    /// <returns>The controller's system information.</returns>
    Task<SystemInfo> GetSystemInfo();

    /// <summary>
    /// Gets the network settings from the LK IHC controller.
    /// </summary>
    /// <returns>The controller's network settings.</returns>
    Task<NetworkSetting> GetNetworkSetting();

    /// <summary>
    /// Gets the DNS servers from the LK IHC controller.
    /// </summary>
    /// <returns>The configured DNS server addresses.</returns>
    Task<string[]> GetDnsServers();

    /// <summary>
    /// Gets the SMTP settings from the LK IHC controller.
    /// </summary>
    /// <returns>The configured SMTP settings.</returns>
    Task<SmtpSettings> GetSmtpSettings();

    /// <summary>
    /// Gets the email settings from the LK IHC controller.
    /// </summary>
    /// <returns>The configured email settings.</returns>
    Task<EmailSettings> GetEmailSettings();

    /// <summary>
    /// Gets the email enable settings from the LK IHC controller.
    /// </summary>
    /// <returns><see langword="true"/> when email control is enabled; otherwise, <see langword="false"/>.</returns>
    Task<bool> GetEmailEnableSettings();

    /// <summary>
    /// Updates the SMTP settings on the LK IHC controller.
    /// </summary>
    /// <param name="settings">The SMTP settings to apply.</param>
    /// <returns><see langword="null"/> when the update request completes.</returns>
    /// <exception cref="HttpRequestException">The controller cannot be reached.</exception>
    Task<ProblemDetails?> UpdateSmtpSettings(SmtpSettings settings);
}

/// <summary>
/// Service for handling configuration settings on the LK IHC controller.
/// </summary>
/// <param name="client">The SOAP client used to communicate with the controller.</param>
/// <param name="authCache">Service for obtaining the current authentication token.</param>
public class ConfigurationService(IClientService client, IAuthCacheService authCache) : IConfigurationService
{
    private const string ServiceName = "ConfigurationService";

    /// <summary>
    /// Gets the DNS servers from the LK IHC controller.
    /// </summary>
    /// <returns>The configured DNS server addresses.</returns>
    /// <exception cref="EmptyResponseException">The controller returns no DNS server data.</exception>
    public async Task<string[]> GetDnsServers()
    {
        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName7, outputMessageName7>(
            ServiceName, "getDNSServers", token!, new inputMessageName7());

        if (response?.getDNSServers1 == null)
        {
            throw new EmptyResponseException();
        }

        var addresses = new List<string>();
        foreach (var item in response.getDNSServers1)
        {
            var longValue = item.ipAddress & 0xFFFFFFFFL;

            // Only take first 4 bytes
            var rawBytes = new byte[4];
            for (var i = 0; i < rawBytes.Length; i++)
            {
                rawBytes[i] = (byte)((longValue >> (i * 8)) & 0xFF);
            }

            // Put bytes in reverse order
            var bytes = new byte[]
            {
                rawBytes[3],
                rawBytes[2],
                rawBytes[1],
                rawBytes[0]
            };

            var ip = new IPAddress(bytes);
            var str = ip.ToString();

            if (!addresses.Contains(str))
            {
                addresses.Add(str);
            }
        }

        return [.. addresses];
    }

    /// <summary>
    /// Gets the SMTP settings from the LK IHC controller.
    /// </summary>
    /// <returns>The configured SMTP settings.</returns>
    /// <exception cref="EmptyResponseException">The controller returns no SMTP settings.</exception>
    public async Task<SmtpSettings> GetSmtpSettings()
    {
        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName5, outputMessageName5>(
            ServiceName, "getSMTPSettings", token!, new inputMessageName5());

        if (response?.getSMTPSettings1 == null)
        {
            throw new EmptyResponseException();
        }

        var info = response.getSMTPSettings1;
        var result = new SmtpSettings
        {
            ServerAddress = info.hostname.Clean(),
            ServerPortNumber = Clean.Int(info.hostport),
            UserName = info.username.Clean(),
            Password = info.password.Clean()
        };

        return result;
    }

    /// <summary>
    /// Gets the email settings from the LK IHC controller.
    /// </summary>
    /// <returns>The configured email settings.</returns>
    /// <exception cref="EmptyResponseException">The controller returns no email settings.</exception>
    public async Task<EmailSettings> GetEmailSettings()
    {
        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName18, outputMessageName18>(
            ServiceName, "getEmailControlSettings", token!, new inputMessageName18());

        if (response?.getEmailControlSettings1 == null)
        {
            throw new EmptyResponseException();
        }

        var info = response.getEmailControlSettings1;
        var result = new EmailSettings
        {
            ServerAddress = info.serverIPAddress.Clean(),
            ServerPortNumber = Clean.Int(info.serverPortNumber),
            EmailAddress = info.emailAddress.Clean(),
            Pop3Username = info.pop3Username.Clean(),
            Pop3Password = info.pop3Password.Clean(),
            PollInterval = Clean.Int(info.pollInterval),
            RemoveEmailsAfterUsage = info.removeEmailsAfterUsage
        };

        return result;
    }

    /// <summary>
    /// Gets the email enable settings from the LK IHC controller.
    /// </summary>
    /// <returns><see langword="true"/> when email control is enabled; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="EmptyResponseException">The controller returns no email enable setting.</exception>
    public async Task<bool> GetEmailEnableSettings()
    {
        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName17, outputMessageName17>(
            ServiceName, "getEmailControlEnabled", token!, new inputMessageName17());

        return response?.getEmailControlEnabled1 == null
            ? throw new EmptyResponseException()
            : response.getEmailControlEnabled1.Value;
    }

    /// <summary>
    /// Gets the network settings from the LK IHC controller.
    /// </summary>
    /// <returns>The controller's network settings.</returns>
    /// <exception cref="EmptyResponseException">The controller returns no network settings.</exception>
    public async Task<NetworkSetting> GetNetworkSetting()
    {
        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName12, outputMessageName12>(
            ServiceName, "getNetworkSettings", token!, new inputMessageName12());

        if (response?.getNetworkSettings1 == null)
        {
            throw new EmptyResponseException();
        }

        var info = response.getNetworkSettings1;
        var result = new NetworkSetting
        {
            IpAddress = info.ipAddress,
            Netmask = info.netmask,
            HttpPort = info.httpPort,
            HttpsPort = info.httpsPort,
            Gateway = info.gateway
        };

        return result;
    }

    /// <summary>
    /// Gets the system information from the LK IHC controller.
    /// </summary>
    /// <returns>The controller's system information.</returns>
    /// <exception cref="EmptyResponseException">The controller returns no system information.</exception>
    public async Task<SystemInfo> GetSystemInfo()
    {
        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName6, outputMessageName6>(
            ServiceName, "getSystemInfo", token!, new inputMessageName6());

        var info = (response?.getSystemInfo1) ?? throw new EmptyResponseException();

        var timeSpan = TimeSpan.FromMilliseconds(info.uptime);
        var uptime = new Uptime
        {
            Time = timeSpan,
            Days = timeSpan.Days,
            Hours = timeSpan.Hours,
            Minutes = timeSpan.Minutes,
            Seconds = timeSpan.Seconds,
            Milliseconds = timeSpan.Milliseconds,
            TotalMilliseconds = info.uptime
        };

        var result = new SystemInfo
        {
            Uptime = uptime,
            Time = info.realtimeclock,
            SerialNumber = info.serialNumber,
            ProductionDate = DateOnly.FromDateTime(info.productionDate),
            Brand = info.brand,
            SoftwareVersion = info.version,
            HardwareVersion = info.hwRevision,
            SoftwareDate = DateOnly.FromDateTime(info.swDate.Date),
            IoVersion = info.datalineVersion,
            RfVersion = info.rfModuleSoftwareVersion,
            RFModuleSerialNumber = info.rfModuleSerialNumber,
            ApplicationIsWithoutViewer = info.applicationIsWithoutViewer
        };

        return result;
    }

    /// <summary>
    /// Updates the SMTP settings on the LK IHC controller.
    /// </summary>
    /// <param name="settings">The SMTP settings to apply.</param>
    /// <returns><see langword="null"/> when the update request completes.</returns>
    /// <exception cref="HttpRequestException">The controller cannot be reached.</exception>
    public async Task<ProblemDetails?> UpdateSmtpSettings(SmtpSettings settings)
    {
        var token = authCache.GetAuthToken().Token;
        var input = new inputMessageName4
        {
            setSMTPSettings1 = new WSSMTPSettings
            {
                hostname = settings.ServerAddress,
                hostport = settings.ServerPortNumber ?? 25,
                username = settings.UserName,
                password = settings.Password
            }
        };

        await client.Post<inputMessageName4, outputMessageName4>(
            ServiceName, "setSMTPSettings", token!, input);

        return null;
    }
}
