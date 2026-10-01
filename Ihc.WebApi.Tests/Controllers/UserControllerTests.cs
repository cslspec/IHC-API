using Ihc.WebApi.Controllers;
using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Ihc.WebApi.Tests.Controllers;

[TestClass]
public class UserControllerTests
{
    private IUserService service = null!;
    private UserController controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        service = Substitute.For<IUserService>();
        controller = new UserController(service);
    }

    [TestMethod]
    public async Task GetUsers_RequestsUsersWithoutPasswords()
    {
        var expected = new[]
        {
            new IhcUser
            {
                Username = "jane",
                Password = string.Empty,
                Firstname = "Jane",
                Lastname = "Doe",
                Phone = string.Empty,
                Group = "Users",
                Project = "Project",
                CreatedDate = DateTimeOffset.UnixEpoch,
                LoginDate = DateTimeOffset.UnixEpoch
            }
        };
        service.GetUsers(includePassword: false).Returns(expected);

        var result = await controller.GetUsers();

        Assert.AreSame(expected, result.Value);
        await service.Received(1).GetUsers(includePassword: false);
    }

    [TestMethod]
    public async Task AddUser_ReturnsNoContentAndForwardsRequest()
    {
        var request = new AddUserRequest { Username = "jane", Password = "secret" };

        var result = await controller.AddUser(request);

        Assert.IsInstanceOfType<NoContentResult>(result);
        await service.Received(1).AddUser(Arg.Is<AddUserRequest>(value => ReferenceEquals(value, request)));
    }

    [TestMethod]
    public async Task UpdateUser_ReturnsNoContentAndForwardsArguments()
    {
        var request = new UpdateUserRequest { Firstname = "Jane" };

        var result = await controller.UpdateUser("jane", request);

        Assert.IsInstanceOfType<NoContentResult>(result);
        await service.Received(1).UpdateUser("jane", Arg.Is<UpdateUserRequest>(value => ReferenceEquals(value, request)));
    }

    [TestMethod]
    public async Task RemoveUser_ReturnsNoContentAndForwardsUsername()
    {
        var result = await controller.RemoveUser("jane");

        Assert.IsInstanceOfType<NoContentResult>(result);
        await service.Received(1).RemoveUser("jane");
    }
}
