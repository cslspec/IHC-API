using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace Ihc.WebApi.Tests.Services;

[TestClass]
public class AuthCacheServiceTests
{
    private IAuthService authService = null!;
    private MemoryCache cache = null!;
    private AuthCacheService service = null!;

    [TestInitialize]
    public void Initialize()
    {
        authService = Substitute.For<IAuthService>();
        cache = new MemoryCache(new MemoryCacheOptions());
        service = new AuthCacheService(authService, cache);
    }

    [TestCleanup]
    public void Cleanup() => cache.Dispose();

    [TestMethod]
    public void GetAuthToken_WhenTokenIsNotCached_LogsInAndCachesToken()
    {
        var user = CreateUser("session-token");
        authService.Login().Returns(user);

        var first = service.GetAuthToken();
        var second = service.GetAuthToken();

        Assert.AreSame(first, second);
        Assert.AreEqual("session-token", first.Token);
        Assert.AreSame(user, first.User);
        Assert.AreNotEqual(Guid.Empty, first.Id);
        Assert.IsTrue(first.ExpireTime > first.LoginTime);
        authService.Received(1).Login();
    }

    [TestMethod]
    public void ClearCache_LogsOutCachedTokenAndNextRequestLogsInAgain()
    {
        var user = CreateUser("first-token");
        authService.Login().Returns(user, CreateUser("second-token"));
        var initialToken = service.GetAuthToken();

        service.ClearCache();
        var replacementToken = service.GetAuthToken();

        Assert.IsNull(initialToken.Token);
        Assert.IsNull(initialToken.User.AuthToken);
        Assert.IsNotNull(initialToken.LogoutTime);
        Assert.AreEqual("second-token", replacementToken.Token);
        authService.Received(1).Logout("first-token");
        authService.Received(2).Login();
    }

    [TestMethod]
    public void ClearCache_WhenNoTokenIsCached_DoesNotLogOut()
    {
        service.ClearCache();

        authService.DidNotReceive().Logout(Arg.Any<string>());
        authService.DidNotReceive().Login();
    }

    private static IhcUser CreateUser(string token) =>
        new()
        {
            Username = "jane",
            Password = "secret",
            Firstname = "Jane",
            Lastname = "Doe",
            Phone = string.Empty,
            Group = "Users",
            Project = "Project",
            CreatedDate = DateTimeOffset.UnixEpoch,
            LoginDate = DateTimeOffset.UnixEpoch,
            AuthToken = token
        };
}
