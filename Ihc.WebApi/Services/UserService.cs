using Ihc.Soap.UserManager;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;

namespace Ihc.WebApi.Services
{
    public interface IUserService
    {
        Task<IhcUser[]> GetUsers(bool includePassword);
    }

    public class UserService(
        IClientService client,
        ISoapDateService dateService,
        IAuthCacheService authCache
        ) : IUserService
    {
        private const string ServiceName = "UserManagerService";

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
