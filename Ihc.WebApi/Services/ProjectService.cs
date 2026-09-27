using Ihc.Soap.Controller;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;

namespace Ihc.WebApi.Services
{
    /// <summary>
    /// Retrieves project availability, metadata, and project data from the IHC controller.
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// Determines whether an IHC project is available on the controller.
        /// </summary>
        /// <returns><see langword="true"/> if a project is available; otherwise, <see langword="false"/>.</returns>
        Task<bool> GetIsProjectAvailable();

        /// <summary>
        /// Retrieves metadata for the current IHC project.
        /// </summary>
        /// <returns>The current project's metadata.</returns>
        /// <exception cref="EmptyResponseException">The controller returns no project information.</exception>
        Task<ProjectInfo> GetProjectInfo();

        /// <summary>
        /// Retrieves and decompresses the current IHC project file.
        /// </summary>
        /// <returns>The project file contents as text.</returns>
        /// <exception cref="EmptyResponseException">The controller returns no project data.</exception>
        Task<string> GetProjectFile();
    }

    /// <summary>
    /// Implements project queries against the IHC controller.
    /// </summary>
    /// <param name="client">The SOAP client used to communicate with the controller.</param>
    /// <param name="authCache">Service for obtaining the current authentication token.</param>
    /// <param name="dateService">Service for converting SOAP dates.</param>
    public class ProjectService(
        IClientService client,
        IAuthCacheService authCache,
        ISoapDateService dateService
        ) : IProjectService
    {
        private const string ServiceName = "ControllerService";

        /// <inheritdoc />
        public async Task<bool> GetIsProjectAvailable()
        {
            var token = authCache.GetAuthToken().Token;
            var response = await client.Post<inputMessageName4, outputMessageName4>(
                ServiceName, "isIHCProjectAvailable", token!, new inputMessageName4());

            var result = response?.isIHCProjectAvailable1 ?? false;
            return result;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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