using Ihc.Soap.UserManager;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;

namespace Ihc.WebApi.Services
{
    /// <summary>
    /// Retrieves user accounts from the IHC controller.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves the controller's users, optionally including their passwords.
        /// </summary>
        /// <param name="includePassword"><see langword="true"/> to include passwords in the results.</param>
        /// <returns>The users returned by the controller.</returns>
        /// <exception cref="EmptyResponseException">The controller returns no user data.</exception>
        Task<IhcUser[]> GetUsers(bool includePassword);
    }

    /// <summary>
    /// Implements user queries against the IHC controller.
    /// </summary>
    /// <param name="client">The SOAP client used to communicate with the controller.</param>
    /// <param name="dateService">Service for converting SOAP dates.</param>
    /// <param name="authCache">Service for obtaining the current authentication token.</param>
    public class UserService(
        IClientService client,
        ISoapDateService dateService,
        IAuthCacheService authCache
        ) : IUserService
    {
        private const string ServiceName = "UserManagerService";

        /// <inheritdoc />
        public async Task<IhcUser[]> GetUsers(bool includePassword)
        {
            var token = authCache.GetAuthToken().Token;
            var response = await client.Post<inputMessageName4, outputMessageName4>(
                ServiceName, "getUsers", token!, new inputMessageName4());

            if (response?.getUsers1 == null)
                throw new EmptyResponseException();

            var users = response.getUsers1.
                Where(x => x != null).
                Select(u => new IhcUser
                {
                    Username = u.username,
                    Password = includePassword ? u.password : string.Empty,
                    Firstname = u.firstname,
                    Lastname = u.lastname,
                    Phone = u.phone,
                    Group = u.group.type,
                    Project = u.project,
                    CreatedDate = dateService.GetDateTime(u.createdDate),
                    LoginDate = dateService.GetDateTime(u.loginDate)
                }).ToArray();

            return users;
        }
    }
}
