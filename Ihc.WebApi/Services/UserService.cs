using Ihc.Soap.UserManager;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;

namespace Ihc.WebApi.Services;

/// <summary>
/// Retrieves and manages user accounts on the IHC controller.
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

    /// <summary>
    /// Creates a new user on the controller.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <exception cref="ArgumentException">A user with the same username already exists.</exception>
    /// <exception cref="EmptyResponseException">The controller returns no user data.</exception>
    Task AddUser(AddUserRequest user);

    /// <summary>
    /// Updates an existing user on the controller.
    /// </summary>
    /// <param name="username">The username of the user to update.</param>
    /// <param name="update">The changes to apply. Properties that are <see langword="null"/> keep their current value.</param>
    /// <exception cref="NotFoundException">No user with the specified username exists.</exception>
    /// <exception cref="EmptyResponseException">The controller returns no user data.</exception>
    Task UpdateUser(string username, UpdateUserRequest update);

    /// <summary>
    /// Removes a user from the controller.
    /// </summary>
    /// <param name="username">The username of the user to remove.</param>
    /// <exception cref="ArgumentException">The user is the administrator or the account used by this API.</exception>
    /// <exception cref="NotFoundException">No user with the specified username exists.</exception>
    /// <exception cref="EmptyResponseException">The controller returns no user data.</exception>
    Task RemoveUser(string username);
}

/// <summary>
/// Implements user queries and updates against the IHC controller.
/// </summary>
/// <param name="client">The SOAP client used to communicate with the controller.</param>
/// <param name="dateService">Service for converting SOAP dates.</param>
/// <param name="authCache">Service for obtaining the current authentication token.</param>
/// <param name="config">The controller connection and login settings.</param>
public class UserService(
    IClientService client,
    ISoapDateService dateService,
    IAuthCacheService authCache,
    IControllerConfiguration config
    ) : IUserService
{
    private const string ServiceName = "UserManagerService";

    private const string AdminUsername = "admin";

    private const string AdministratorsGroup = "text.usermanager.group_administrators";
    private const string UsersGroup = "gtext.users";

    /// <inheritdoc />
    public async Task<IhcUser[]> GetUsers(bool includePassword)
    {
        var users = (await GetControllerUsers()).
            Select(u => new IhcUser
            {
                Username = u.username,
                Password = includePassword ? u.password : string.Empty,
                Email = u.email,
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

    /// <inheritdoc />
    public async Task AddUser(AddUserRequest user)
    {
        var users = await GetControllerUsers();
        if (users.Any(u => u.username == user.Username))
        {
            throw new ArgumentException($"A user named '{user.Username}' already exists.", nameof(user));
        }

        var input = new inputMessageName3
        {
            addUser1 = new WSUser
            {
                username = user.Username,
                password = user.Password,
                email = user.Email ?? string.Empty,
                firstname = user.Firstname ?? string.Empty,
                lastname = user.Lastname ?? string.Empty,
                phone = user.Phone ?? string.Empty,
                group = new WSUserGroup { type = GetGroupType(user.Group) }
            }
        };

        var token = authCache.GetAuthToken().Token;
        await client.Post<inputMessageName3, outputMessageName3>(
            ServiceName, "addUser", token!, input);
    }

    /// <inheritdoc />
    public async Task UpdateUser(string username, UpdateUserRequest update)
    {
        var users = await GetControllerUsers();
        var current = users.FirstOrDefault(u => u.username == username)
            ?? throw new NotFoundException($"No user named '{username}' exists.");

        // The controller replaces the entire user, so unchanged values are copied
        // from the current user. The dates and project are managed by the controller.
        var input = new inputMessageName1
        {
            updateUser1 = new WSUser
            {
                username = current.username,
                password = update.Password ?? current.password,
                email = update.Email ?? current.email,
                firstname = update.Firstname ?? current.firstname,
                lastname = update.Lastname ?? current.lastname,
                phone = update.Phone ?? current.phone,
                group = update.Group != null
                    ? new WSUserGroup { type = GetGroupType(update.Group.Value) }
                    : current.group
            }
        };

        var token = authCache.GetAuthToken().Token;
        await client.Post<inputMessageName1, outputMessageName1>(
            ServiceName, "updateUser", token!, input);
    }

    /// <inheritdoc />
    public async Task RemoveUser(string username)
    {
        if (string.Equals(username, AdminUsername, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("The administrator cannot be removed.", nameof(username));
        }

        // Removing the account this API logs in with would lock the API out of the controller.
        if (username == config.UserName)
        {
            throw new ArgumentException("The user used by this API cannot be removed.", nameof(username));
        }

        var users = await GetControllerUsers();
        if (!users.Any(u => u.username == username))
        {
            throw new NotFoundException($"No user named '{username}' exists.");
        }

        var token = authCache.GetAuthToken().Token;
        await client.Post<inputMessageName2, outputMessageName2>(
            ServiceName, "removeUser", token!, new inputMessageName2(username));
    }

    /// <summary>
    /// Retrieves the users from the controller, including their passwords.
    /// </summary>
    /// <returns>The users returned by the controller.</returns>
    /// <exception cref="EmptyResponseException">The controller returns no user data.</exception>
    private async Task<WSUser[]> GetControllerUsers()
    {
        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName4, outputMessageName4>(
            ServiceName, "getUsers", token!, new inputMessageName4());

        if (response?.getUsers1 == null)
        {
            throw new EmptyResponseException();
        }

        return response.getUsers1.Where(x => x != null).ToArray();
    }

    private static string GetGroupType(UserGroup group) => group switch
    {
        UserGroup.Administrators => AdministratorsGroup,
        _ => UsersGroup
    };
}
