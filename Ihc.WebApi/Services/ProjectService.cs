using Ihc.Soap.Controller;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;

namespace Ihc.WebApi.Services
{
    public interface IProjectService
    {
        Task<bool> GetIsProjectAvailable();

        Task<ProjectInfo> GetProjectInfo();

        Task<string> GetProjectFile();
    }

    public class ProjectService(
        IClientService client,
        IAuthCacheService authCache,
        ISoapDateService dateService
        ) : IProjectService
    {
        private const string ServiceName = "ControllerService";

        public async Task<bool> GetIsProjectAvailable()
        {
            var token = authCache.GetAuthToken().Token;
            var response = await client.Post<inputMessageName4, outputMessageName4>(
                ServiceName, "isIHCProjectAvailable", token!, new inputMessageName4());

            var result = response?.isIHCProjectAvailable1 ?? false;
            return result; 
        }

        public async Task<ProjectInfo> GetProjectInfo()
        {
            var token = authCache.GetAuthToken().Token;
            var response = await client.Post<inputMessageName11, outputMessageName11>(
                ServiceName, "getProjectInfo", token!, new inputMessageName11());

            if (response?.getProjectInfo1 == null)
                throw new EmptyResponseException();

            var info = response.getProjectInfo1;
            var result = new ProjectInfo
            {
                CustomerName = info.customerName,
                InstallerName = info.installerName,
                ProjectNumber = info.projectNumber,
                ProjectMajorRevision = info.projectMajorRevision,
                ProjectMinorRevision = info.projectMinorRevision,
                VisualMajorVersion = info.visualMajorVersion,
                VisualMinorVersion = info.visualMinorVersion,
                Lastmodified = dateService.GetDateTime(info.lastmodified)
            };

            return result;
        }

        public async Task<string> GetProjectFile()
        {
            var token = authCache.GetAuthToken().Token;
            var response = await client.Post<inputMessageName12, outputMessageName12>(
                ServiceName, "getIHCProject", token!, new inputMessageName12());

            if (response?.getIHCProject1 == null)
                throw new EmptyResponseException();

            using MemoryStream mscompressed = new(response.getIHCProject1.data);
            using Stream inStream = new System.IO.Compression.GZipStream(mscompressed, System.IO.Compression.CompressionMode.Decompress);
            using StreamReader reader = new(inStream, System.Text.Encoding.GetEncoding("ISO-8859-1"));
            var text = await reader.ReadToEndAsync();

            return text;
        }
    }
}