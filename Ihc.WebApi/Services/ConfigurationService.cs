using Ihc.Soap.Configuration;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Extensions;
using Ihc.WebApi.Model;
using Ihc.WebApi.Util;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ihc.WebApi.Services
{
    public interface IConfigurationService
    {
        Task<SystemInfo> GetSystemInfo();

        Task<NetworkSetting> GetNetworkSetting();

        Task<string[]> GetDnsServers();

        Task<SmtpSettings> GetSmtpSettings();

        Task<EmailSettings> GetEmailSettings();

        Task<bool> GetEmailEnableSettings();
        
        Task<ProblemDetails?> UpdateSmtpSettings(SmtpSettings settings);
    }

    public class ConfigurationService(
        IClientService client,
        IAuthCacheService authCache
        ) : IConfigurationService
    {

        private const string ServiceName = "ConfigurationService";

        public async Task<string[]> GetDnsServers()
        {
            var token = authCache.GetAuthToken().Token;
            var response = await client.Post<inputMessageName7, outputMessageName7>(
                ServiceName, "getDNSServers", token!, new inputMessageName7());

            if (response?.getDNSServers1 == null)
                throw new EmptyResponseException();

            var addresses = new List<string>();
            foreach (var item in response.getDNSServers1)
            {
                var longValue = item.ipAddress & 0xFFFFFFFFL;

                // Only take first 4 bytes
                byte[] rawBytes = new byte[4];
                for (int i = 0; i < rawBytes.Length; i++)
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

            return addresses.ToArray();
        }

        public async Task<SmtpSettings> GetSmtpSettings()
        {
            var token = authCache.GetAuthToken().Token;
            var response = await client.Post<inputMessageName5, outputMessageName5>(
                ServiceName, "getSMTPSettings", token!, new inputMessageName5());

            if (response?.getSMTPSettings1 == null)
                throw new EmptyResponseException();

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

        public async Task<EmailSettings> GetEmailSettings()
        {
            var token = authCache.GetAuthToken().Token;
            var response = await client.Post<inputMessageName18, outputMessageName18>(
                ServiceName, "getEmailControlSettings", token!, new inputMessageName18());

            if (response?.getEmailControlSettings1 == null)
                throw new EmptyResponseException();

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

        public async Task<bool> GetEmailEnableSettings()
        {
            var token = authCache.GetAuthToken().Token;
            var response = await client.Post<inputMessageName17, outputMessageName17>(
                ServiceName, "getEmailControlEnabled", token!, new inputMessageName17());

            if (response?.getEmailControlEnabled1 == null)
                throw new EmptyResponseException();

            return response.getEmailControlEnabled1.Value;
        }

        public async Task<NetworkSetting> GetNetworkSetting()
        {
            var token = authCache.GetAuthToken().Token;
            var response = await client.Post<inputMessageName12, outputMessageName12>(
                ServiceName, "getNetworkSettings", token!, new inputMessageName12());

            if (response?.getNetworkSettings1 == null)
                throw new EmptyResponseException();

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

        public async Task<SystemInfo> GetSystemInfo()
        {
            var token = authCache.GetAuthToken().Token;
            var response = await client.Post<inputMessageName6, outputMessageName6>(
                ServiceName, "getSystemInfo", token!, new inputMessageName6());

            var info = response?.getSystemInfo1;
            if (info == null)
                throw new EmptyResponseException();

            var timeSpan = TimeSpan.FromMilliseconds(response.getSystemInfo1.uptime);
            var uptime = new Uptime
            {
                Time = timeSpan,
                Days = timeSpan.Days,
                Hours = timeSpan.Hours,
                Minutes = timeSpan.Minutes,
                Seconds = timeSpan.Seconds,
                Milliseconds = timeSpan.Milliseconds,
                TotalMilliseconds = response.getSystemInfo1.uptime
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

            try
            {
                await client.Post<inputMessageName4, outputMessageName4>(
                    ServiceName, "setSMTPSettings", token!, input);
            }
            catch (Exception ex)
            {
                return new ProblemDetails
                {
                    Title = "Request partially successful",
                    Detail = "Some items could not be processed.",
                };
            }

            return null;
        }
    }
}
