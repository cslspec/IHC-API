using Ihc.Soap.Authentication;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;
using Ihc.WebApi.Model.Envelope;
using System.Text;

namespace Ihc.WebApi.Services
{
    public interface IAuthService
    {
        IhcUser Login();

        bool? Logout(string token);
    }

    public class AuthService(
        IControllerConfiguration config,
        ISoapDateService dateService,
        IXmlService xmlService,
        IHttpClientFactory clientFactory)
        : IAuthService
    {
        public IhcUser Login()
        {
            var authRequest = new inputMessageName2
            {
                authenticate1 = new WSAuthenticationData
                {
                    username = config.UserName,
                    password = config.Password,
                    application = config.Application
                }
            };

            var xmlObject = new RequestEnvelope<inputMessageName2>(authRequest);
            var xml = xmlService.SerializeXml(xmlObject);
            var content = new StringContent(xml, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", "authenticate");
            content.Headers.Add("UserAgent", "HomeAutomation");

            var url = config.Address + "/ws/AuthenticationService";
            var client = clientFactory.CreateClient();

            var response = client.
                PostAsync(url, content).
                ConfigureAwait(false).
                GetAwaiter().
                GetResult();

            response.EnsureSuccessStatusCode();
            var cookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault();

            string responseString = response.Content.
                ReadAsStringAsync().
                ConfigureAwait(false).
                GetAwaiter().
                GetResult();

            var respObject = xmlService.DeserializeXml<ResponseEnvelope<outputMessageName2>>(responseString);

            var result = respObject?.Body.authenticate2;
            if (result == null)
            {
                throw new HttpRequestException("No result returned.");
            }

            if (result.loginWasSuccessful)
            {
                var user = new IhcUser
                {
                    Username = result.loggedInUser.username,
                    Password = result.loggedInUser.password,
                    Firstname = result.loggedInUser.firstname,
                    Lastname = result.loggedInUser.lastname,
                    Phone = result.loggedInUser.phone,
                    Group = result.loggedInUser.group.type,
                    Project = result.loggedInUser.project,
                    CreatedDate = dateService.GetDateTime(result.loggedInUser.createdDate),
                    LoginDate = dateService.GetDateTime(result.loggedInUser.loginDate),
                    AuthToken = cookie
                };

                return user;
            }
            else if (result.loginFailedDueToAccountInvalid)
            {
                throw new ErrorWithCodeException(Errors.LOGIN_FAILED_DUE_TO_ACCOUNT_INVALID_ERROR, "Ihc server login reports invalid account for " + url);
            }
            else if (result.loginFailedDueToConnectionRestrictions)
            {
                throw new ErrorWithCodeException(Errors.LOGIN_FAILED_DUE_TO_CONNECTION_RESTRUCTIONS_ERROR, "Ihc server login reports connection restriction error for " + url);
            }
            else if (result.loginFailedDueToInsufficientUserRights)
            {
                throw new ErrorWithCodeException(Errors.LOGIN_FAILED_DUE_TO_INSUFFICIENT_USER_RIGHTS_ERROR, "Ihc server login reports invalid account for " + url);
            }
            else
            {
                throw new ErrorWithCodeException(Errors.LOGIN_UNKNOWN_ERROR, "Ihc server failed login for " + url);
            }
        }

        public bool? Logout(string token)
        {
            var xmlObject = new RequestEnvelope<inputMessageName3>(new inputMessageName3());
            var xml = xmlService.SerializeXml(xmlObject);
            var content = new StringContent(xml, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", "disconnect");
            content.Headers.Add("UserAgent", "HomeAutomation");
            content.Headers.Add("Cookie", token);

            var url = config.Address + "/ws/AuthenticationService";
            var client = clientFactory.CreateClient();

            var response = client.
                PostAsync(url, content).
                ConfigureAwait(false).
                GetAwaiter().
                GetResult();

            string responseString = response.Content.
                ReadAsStringAsync().
                ConfigureAwait(false).
                GetAwaiter().
                GetResult();

            var respObject = xmlService.DeserializeXml<ResponseEnvelope<outputMessageName3>>(responseString);

            var result = respObject?.Body.disconnect1;
            return result;
        }
    }
}
