using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;
using Ihc.WebApi.Model.Envelope;
using System.Text;

namespace Ihc.WebApi.Services
{
    /// <summary>
    /// Sends authenticated SOAP requests to the IHC controller.
    /// </summary>
    public interface IClientService
    {
        /// <summary>
        /// Posts a SOAP request and deserializes the response body.
        /// </summary>
        /// <typeparam name="T">The SOAP request type.</typeparam>
        /// <typeparam name="U">The SOAP response body type.</typeparam>
        /// <param name="service">The SOAP service endpoint name.</param>
        /// <param name="action">The SOAP action to invoke.</param>
        /// <param name="token">The authentication token for the request.</param>
        /// <param name="request">The request body to serialize.</param>
        /// <returns>The deserialized SOAP response body.</returns>
        /// <exception cref="HttpRequestException">The controller cannot be reached or returns a non-success HTTP status.</exception>
        /// <exception cref="EmptyResponseException">The response envelope is empty.</exception>
        Task<U> Post<T, U>(string service, string action, string token, T request);
    }

    /// <summary>
    /// Implements SOAP request posting to the IHC controller.
    /// </summary>
    /// <param name="config">The controller connection settings.</param>
    /// <param name="xmlService">Service for serializing and deserializing SOAP messages.</param>
    /// <param name="clientFactory">Factory for creating HTTP clients.</param>
    public class ClientService(
        IControllerConfiguration config,
        IXmlService xmlService,
        IHttpClientFactory clientFactory
        ) : IClientService
    {

        private const string ClientName = "HomeAutomation";

        /// <inheritdoc />
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

            using var response = await client.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"IHC SOAP request '{service}/{action}' returned HTTP {(int)response.StatusCode} ({response.ReasonPhrase}).",
                    null,
                    response.StatusCode);
            }

            var responseString = await response.Content.ReadAsStringAsync();

            var respObject = xmlService.DeserializeXml<ResponseEnvelope<U>>(responseString)
                ?? throw new EmptyResponseException();

            var result = respObject.Body;
            return result;
        }
    }
}
