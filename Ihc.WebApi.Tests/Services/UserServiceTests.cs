using Ihc.Soap.UserManager;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using NSubstitute;

namespace Ihc.WebApi.Tests.Services;

[TestClass]
public class UserServiceTests
{
    private IClientService client = null!;
    private ISoapDateService dateService = null!;
    private IAuthCacheService authCache = null!;
    private IControllerConfiguration config = null!;
    private UserService service = null!;

    [TestInitialize]
    public void Initialize()
    {
        client = Substitute.For<IClientService>();
        dateService = Substitute.For<ISoapDateService>();
        authCache = Substitute.For<IAuthCacheService>();
        config = Substitute.For<IControllerConfiguration>();
        service = new UserService(client, dateService, authCache, config);

        var token = Substitute.For<IAuthToken>();
        token.Token.Returns("session-token");
        authCache.GetAuthToken().Returns(token);
        dateService.GetDateTime(Arg.Any<WSDate>()).Returns(new DateTime(2024, 1, 2));
    }

    [TestMethod]
    public async Task GetUsers_ExcludesPasswordsWhenNotRequested()
    {
        ConfigureUsers(CreateUser("jane", "secret"));

        var users = await service.GetUsers(includePassword: false);

        Assert.AreEqual(1, users.Length);
        Assert.AreEqual("jane", users[0].Username);
        Assert.AreEqual(string.Empty, users[0].Password);
    }

    [TestMethod]
    public async Task GetUsers_IncludesPasswordsWhenRequested()
    {
        ConfigureUsers(CreateUser("jane", "secret"));

        var users = await service.GetUsers(includePassword: true);

        Assert.AreEqual("secret", users[0].Password);
    }

    [TestMethod]
    public async Task AddUser_WhenUsernameAlreadyExists_ThrowsAndDoesNotCreateUser()
    {
        ConfigureUsers(CreateUser("jane", "secret"));
        var request = new AddUserRequest { Username = "jane", Password = "new-secret" };

        await Assert.ThrowsExactlyAsync<ArgumentException>(() => service.AddUser(request));

        await client.DidNotReceive().Post<inputMessageName3, outputMessageName3>(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<inputMessageName3>());
    }

    [TestMethod]
    public async Task AddUser_WhenUsernameIsNew_SendsUserWithDefaultValues()
    {
        ConfigureUsers();

        await service.AddUser(new AddUserRequest { Username = "jane", Password = "secret" });

        await client.Received(1).Post<inputMessageName3, outputMessageName3>(
            "UserManagerService",
            "addUser",
            "session-token",
            Arg.Is<inputMessageName3>(input =>
                input.addUser1.username == "jane" &&
                input.addUser1.password == "secret" &&
                input.addUser1.email == string.Empty &&
                input.addUser1.firstname == string.Empty &&
                input.addUser1.lastname == string.Empty &&
                input.addUser1.phone == string.Empty &&
                input.addUser1.group.type == "gtext.users"));
    }

    [TestMethod]
    public async Task GetUsers_WhenControllerReturnsNoUsers_ThrowsEmptyResponseException()
    {
        client.Post<inputMessageName4, outputMessageName4>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName4>())
            .Returns(Task.FromResult(new outputMessageName4 { getUsers1 = null! }));

        await Assert.ThrowsExactlyAsync<EmptyResponseException>(
            () => service.GetUsers(includePassword: false));
    }

    [TestMethod]
    public async Task UpdateUser_WhenFieldsAreOmitted_PreservesExistingValues()
    {
        ConfigureUsers(CreateUser("jane", "old-secret"));

        await service.UpdateUser("jane", new UpdateUserRequest { Firstname = "Janet" });

        await client.Received(1).Post<inputMessageName1, outputMessageName1>(
            "UserManagerService",
            "updateUser",
            "session-token",
            Arg.Is<inputMessageName1>(input =>
                input.updateUser1.username == "jane" &&
                input.updateUser1.password == "old-secret" &&
                input.updateUser1.firstname == "Janet" &&
                input.updateUser1.email == "jane@example.com" &&
                input.updateUser1.group.type == "gtext.users"));
    }

    [TestMethod]
    public async Task UpdateUser_WhenUserDoesNotExist_ThrowsNotFoundException()
    {
        ConfigureUsers();

        await Assert.ThrowsExactlyAsync<NotFoundException>(
            () => service.UpdateUser("missing", new UpdateUserRequest()));

        await client.DidNotReceive().Post<inputMessageName1, outputMessageName1>(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<inputMessageName1>());
    }

    [TestMethod]
    public async Task RemoveUser_WhenUsernameIsAdmin_ThrowsWithoutCallingController()
    {
        await Assert.ThrowsExactlyAsync<ArgumentException>(() => service.RemoveUser("ADMIN"));

        await client.DidNotReceive().Post<inputMessageName4, outputMessageName4>(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<inputMessageName4>());
    }

    [TestMethod]
    public async Task RemoveUser_WhenUsernameIsConfiguredAccount_ThrowsWithoutCallingController()
    {
        config.UserName.Returns("jane");

        await Assert.ThrowsExactlyAsync<ArgumentException>(() => service.RemoveUser("jane"));

        await client.DidNotReceive().Post<inputMessageName4, outputMessageName4>(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<inputMessageName4>());
    }

    [TestMethod]
    public async Task RemoveUser_WhenUserExists_SendsUsernameToController()
    {
        ConfigureUsers(CreateUser("jane", "secret"));

        await service.RemoveUser("jane");

        await client.Received(1).Post<inputMessageName2, outputMessageName2>(
            "UserManagerService",
            "removeUser",
            "session-token",
            Arg.Is<inputMessageName2>(input => input.removeUser1 == "jane"));
    }

    private void ConfigureUsers(params WSUser[] users)
    {
        client.Post<inputMessageName4, outputMessageName4>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName4>())
            .Returns(Task.FromResult(new outputMessageName4(users)));
    }

    private static WSUser CreateUser(string username, string password)
    {
        return new WSUser
        {
            username = username,
            password = password,
            email = "jane@example.com",
            firstname = "Jane",
            lastname = "Doe",
            phone = "555-0100",
            group = new WSUserGroup { type = "gtext.users" },
            project = "home",
            createdDate = new WSDate(),
            loginDate = new WSDate()
        };
    }
}
