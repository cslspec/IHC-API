using Ihc.Soap.Authentication;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;
using Ihc.WebApi.Model.Envelope;
using System.Text;

namespace Ihc.WebApi.Services;

/// <summary>
/// Authenticates with the IHC controller and manages authenticated sessions.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Logs in to the IHC controller using the configured credentials.
    /// </summary>
    /// <returns>The authenticated user and session token.</returns>
    /// <exception cref="AuthorizationException">The controller rejects the login or cannot be reached.</exception>
    IhcUser Login();

    /// <summary>
    /// Logs out the specified session from the IHC controller.
    /// </summary>
    /// <param name="token">The authentication token for the session to end.</param>
    /// <returns>The controller's logout result, or <see langword="null"/> if no result is returned.</returns>
    bool? Logout(string token);
}

/// <summary>
/// Implements authentication and logout requests for the IHC controller.
/// </summary>
/// <param name="config">The controller connection and login settings.</param>
/// <param name="dateService">Service for converting SOAP dates.</param>
/// <param name="xmlService">Service for serializing and deserializing SOAP messages.</param>
/// <param name="clientFactory">Factory for creating HTTP clients.</param>
public class AuthService(
        IControllerConfiguration config,
        ISoapDateService dateService,
        IXmlService xmlService,
        IHttpClientFactory clientFactory)
    : IAuthService
{
    /// <summary>
    /// IHC's legacy authentication protocol uses this as an application/user-group identifier.
    /// The value "openapi" is not related to the OpenAPI specification. Always use "openapi"
    /// for third-party authentication requests to the IHC controller.
    /// </summary>
    private const string Application = "openapi";

    /// <inheritdoc />
    public IhcUser Login()
    {
        var authRequest = new inputMessageName2
        {
            authenticate1 = new WSAuthenticationData
            {
                username = config.UserName,
                password = config.Password,
                application = Application
            }
        };

        var xmlObject = new RequestEnvelope<inputMessageName2>(authRequest);
        var xml = xmlService.SerializeXml(xmlObject);
        var content = new StringContent(xml, Encoding.UTF8, "text/xml");
        content.Headers.Add("SOAPAction", "authenticate");
        content.Headers.Add("UserAgent", "HomeAutomation");

        var url = config.Address + "/ws/AuthenticationService";
        var client = clientFactory.CreateClient();

        string? responseString = null;
        string? cookie = null;
        try
        {
            using var response = client.
                PostAsync(url, content).
                ConfigureAwait(false).
                GetAwaiter().
                GetResult();

            response.EnsureSuccessStatusCode();
            cookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault();

            responseString = response.Content.
                ReadAsStringAsync().
                ConfigureAwait(false).
                GetAwaiter().
                GetResult();
        }
        catch (HttpRequestException ex)
        {
            throw new AuthorizationException("IHC server login failed due to connection error for " + url, ex);
        }
        catch (Exception ex)
        {
            throw new AuthorizationException("IHC server login failed for " + url, ex);
        }

        var respObject = xmlService.DeserializeXml<ResponseEnvelope<outputMessageName2>>(responseString);

        var result = (respObject?.Body.authenticate2)
            ?? throw new AuthorizationException("No authorization result returned.");

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
            var message = $"IHC server login reports invalid account for {url}";
            throw new AuthorizationException(message, CommunicationErrors.AccountInvalid);
        }
        else if (result.loginFailedDueToConnectionRestrictions)
        {
            var message = $"IHC server login reports connection restriction for {url}";
            throw new AuthorizationException(message, CommunicationErrors.ConnectionRestriction);
        }
        else if (result.loginFailedDueToInsufficientUserRights)
        {
            var message = $"IHC server login reports insufficient user rights for {url}";
            throw new AuthorizationException(message, CommunicationErrors.UserRights);
        }
        else
        {
            var message = $"IHC server login failed for {url}";
            throw new AuthorizationException(message, CommunicationErrors.UnknownError);
        }
    }

    /// <inheritdoc />
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

        using var response = client.
            PostAsync(url, content).
            ConfigureAwait(false).
            GetAwaiter().
            GetResult();

        var responseString = response.Content.
            ReadAsStringAsync().
            ConfigureAwait(false).
            GetAwaiter().
            GetResult();

        var respObject = xmlService.DeserializeXml<ResponseEnvelope<outputMessageName3>>(responseString);

        var result = respObject?.Body.disconnect1;
        return result;
    }
}
