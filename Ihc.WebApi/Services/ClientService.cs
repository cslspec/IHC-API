using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;
using Ihc.WebApi.Model.Envelope;
using System.Text;

namespace Ihc.WebApi.Services
{
    public interface IClientService
    {
        Task<U> Post<T, U>(string service, string action, string token, T request);
    }

    public class ClientService(
        IControllerConfiguration config,
        IXmlService xmlService,
        IHttpClientFactory clientFactory
        ) : IClientService
    {

        private const string ClientName = "HomeAutomation";

        public async Task<U> Post<T, U>(string service, string action, string token, T request)
        {
            var xmlObject = new RequestEnvelope<T>(request);
            var xml = xmlService.SerializeXml(xmlObject);
            var content = new StringContent(xml, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", action);
            content.Headers.Add("UserAgent", ClientName);
            content.Headers.Add("Cookie", token);

            var url = config.Address + "/ws/" + service;
            var client = clientFactory.CreateClient();

            var response = await client.PostAsync(url, content);
            string responseString = await response.Content.ReadAsStringAsync();

            var respObject = xmlService.DeserializeXml<ResponseEnvelope<U>>(responseString);
            if (respObject == null)
            {
                throw new EmptyResponseException();
            }

            var result = respObject.Body;
            return result;
        }
    }
}
