using Ihc.Soap.Configuration;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;

namespace Ihc.WebApi.Services
{
    public interface IAccessControlService
    {
        Task<AccessControlSetting[]> GetAccessControl();
    }

    public class AccessControlService(
        IClientService client,
        IAuthCacheService authCache
        ) : IAccessControlService
    {

        private const string ServiceName = "ConfigurationService";

        public async Task<AccessControlSetting[]> GetAccessControl()
        {
            var token = authCache.GetAuthToken().Token;
            var response = await client.Post<inputMessageName14, outputMessageName14>(
                ServiceName, "getWebAccessControl", token!, new inputMessageName14());

            var info = response?.getWebAccessControl1;
            if (info == null)
                throw new EmptyResponseException();

            var result = GetAccessControlList(info);
            return result;
        }

        private AccessControlSetting[] GetAccessControlList(WSAccessControl info)
        {
            var list = new List<AccessControlSetting>()
            {
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
            };

            return list.ToArray();

        }
    }
}
