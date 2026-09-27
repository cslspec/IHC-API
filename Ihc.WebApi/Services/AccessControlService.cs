using Ihc.Soap.Configuration;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;

namespace Ihc.WebApi.Services;

/// <summary>
/// Provides functionality for retrieving access control settings.
/// </summary>
public interface IAccessControlService
{
    /// <summary>
    /// Retrieves the current access control settings from the external service.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an array
    /// of access control settings.
    /// </returns>
    /// <exception cref="EmptyResponseException">Thrown when the external service returns an empty response.</exception>
    Task<AccessControlSetting[]> GetAccessControl();
}

/// <summary>
/// Service for managing and retrieving access control settings from an external IHC service.
/// </summary>
/// <remarks>
/// This service acts as a bridge between the web API and the external IHC configuration service.
/// It retrieves access control settings, processes them, and exposes them through a standardized
/// format. The service requires authentication via a cached auth token.
/// </remarks>
public class AccessControlService : IAccessControlService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AccessControlService"/> class.
    /// </summary>
    /// <param name="client">The SOAP client used to communicate with the external service.</param>
    /// <param name="authCache">The authentication cache service for retrieving auth tokens.</param>
    /// <param name="logger">The logger for recording diagnostic and error information.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is <c>null</c>.</exception>
    public AccessControlService(
        IClientService client,
        IAuthCacheService authCache,
        ILogger<AccessControlService> logger)
    {
        this.client = client;
        this.authCache = authCache;
        this.logger = logger;
    }

    /// <summary>
    /// The name of the external SOAP service providing access control configuration.
    /// </summary>
    private const string ServiceName = "ConfigurationService";

    /// <summary>
    /// The SOAP client used to communicate with the external service.
    /// </summary>
    private readonly IClientService client;

    /// <summary>
    /// The authentication cache service for retrieving cached authentication tokens.
    /// </summary>
    private readonly IAuthCacheService authCache;

    /// <summary>
    /// The logger for recording diagnostic, warning, and error information.
    /// </summary>
    private readonly ILogger<AccessControlService> logger;

    /// <summary>
    /// Retrieves the current access control settings from the external service.
    /// </summary>
    /// <remarks>
    /// This method performs an asynchronous call to an external SOAP service to obtain access
    /// control settings. It uses a cached authentication token to authorize the request.
    ///
    /// If the service returns <c>null</c> or empty data, an <see cref="EmptyResponseException"/> is thrown.
    /// If an exception occurs while processing the response, it is logged and an empty array is returned.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an array
    /// of access control settings representing the current access permissions.
    /// </returns>
    /// <exception cref="EmptyResponseException">Thrown when the external service returns an empty response.</exception>
    public async Task<AccessControlSetting[]> GetAccessControl()
    {
        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName14, outputMessageName14>(
            ServiceName, "getWebAccessControl", token!, new inputMessageName14());

        var info = response?.getWebAccessControl1;
        if (info == null)
        {
            logger.LogError("Got an empty response from external getWebAccessControl");
            throw new EmptyResponseException();
        }

        logger.LogDebug("Retrieved access control settings");

        try
        {
            var result = GetAccessControlList(info);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Invalid access control settings from external service");
        }

        return [];
    }

    /// <summary>
    /// Builds and returns the access control list based on the provided access control information.
    /// </summary>
    /// <remarks>
    /// This private method maps the external service's access control data to the standardized
    /// <see cref="AccessControlSetting"/> format used by the API. It creates settings for the following categories:
    /// <list type="bullet">
    /// <item><description>Administrator - LK IHC Administrator access</description></item>
    /// <item><description>IHC Visual - LK IHC Visual application access</description></item>
    /// <item><description>Documentation - Online reports access</description></item>
    /// <item><description>Open API - Third-party product integration access</description></item>
    /// <item><description>Scene Design - LK IHC SceneDesign access</description></item>
    /// <item><description>Scene View - LK IHC SceneView access</description></item>
    /// <item><description>Server Status - LK IHC ServiceView access</description></item>
    /// <item><description>Tree View - Unofficial Tree View access</description></item>
    /// <item><description>Web Scene View - LK IHC WebSceneView access</description></item>
    /// <item><description>USB - USB login requirement flag</description></item>
    /// </list>
    ///
    /// Each setting includes external, internal, and USB access permission flags.
    /// </remarks>
    /// <param name="info">
    /// The access control information object from the external service containing permission flags
    /// for each access category and access method (external, internal, USB).
    /// </param>
    /// <returns>
    /// An array of access control settings representing the current access permissions for each category.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="info"/> is <c>null</c>.</exception>
    private AccessControlSetting[] GetAccessControlList(WSAccessControl info)
    {
        ArgumentNullException.ThrowIfNull(info, nameof(info));

        List<AccessControlSetting> list =
        [
            new()
            {
                Id = 1,
                Name = "Administrator",
                Description = "LK IHC Administrator",
                External = info.m_administrator_external,
                Internal = info.m_administrator_internal,
                Usb = info.m_administrator_usb
            },
            new()
            {
                Id = 2,
                Name = "IHC Visual",
                Description = "LK IHC Visual",
                External = info.m_ihcvisual_external,
                Internal = info.m_ihcvisual_internal,
                Usb = info.m_ihcvisual_usb
            },
            new()
            {
                Id = 3,
                Name = "Documentation",
                Description = "Online reports",
                External = info.m_onlinedocumentation_external,
                Internal = info.m_onlinedocumentation_internal,
                Usb = info.m_onlinedocumentation_usb
            },
            new()
            {
                Id = 4,
                Name = "Open API",
                Description = "Open for third party products",
                External = info.m_openapi_external,
                Internal = info.m_openapi_internal,
                Usb = info.m_openapi_usb
            },
            new()
            {
                Id = 5,
                Name = "Scene Design",
                Description = "LK IHC SceneDesign",
                External = info.m_scenedesign_external,
                Internal = info.m_scenedesign_internal,
                Usb = info.m_scenedesign_usb
            },
            new()
            {
                Id = 6,
                Name = "Scene View",
                Description = "LK IHC SceneView",
                External = info.m_sceneview_external,
                Internal = info.m_sceneview_internal,
                Usb = info.m_sceneview_usb
            },
            new()
            {
                Id = 7,
                Name = "Server Status",
                Description = "LK IHC ServiceView",
                External = info.m_serverstatus_external,
                Internal = info.m_serverstatus_internal,
                Usb = info.m_serverstatus_usb
            },
            new()
            {
                Id = 8,
                Name = "Tree View",
                Description = "Unofficial Tree View",
                External = info.m_treeview_external,
                Internal = info.m_treeview_internal,
                Usb = info.m_treeview_usb
            },
            new()
            {
                Id = 9,
                Name = "Web Scene View",
                Description = "LK IHC WebSceneView",
                External = info.m_websceneview_external,
                Internal = info.m_websceneview_internal,
                Usb = info.m_websceneview_usb
            },
            new()
            {
                Id = 10,
                Name = "USB",
                Description = "USB Login Required",
                External = false,
                Internal = false,
                Usb = info.m_usbLoginRequired_usb,
            }
        ];

        logger.LogDebug("Build list of access control settings.");
        return [.. list];
    }
}
