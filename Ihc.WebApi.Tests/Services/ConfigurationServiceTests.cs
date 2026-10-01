using Ihc.Soap.Configuration;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;
using Ihc.WebApi.Services;
using NSubstitute;

namespace Ihc.WebApi.Tests.Services;

[TestClass]
public class ConfigurationServiceTests
{
    private IClientService client = null!;
    private IAuthCacheService authCache = null!;
    private Ihc.WebApi.Services.ConfigurationService service = null!;

    [TestInitialize]
    public void Initialize()
    {
        client = Substitute.For<IClientService>();
        authCache = Substitute.For<IAuthCacheService>();
        service = new Ihc.WebApi.Services.ConfigurationService(client, authCache);

        var token = Substitute.For<IAuthToken>();
        token.Token.Returns("session-token");
        authCache.GetAuthToken().Returns(token);
    }

    [TestMethod]
    public async Task GetDnsServers_ConvertsAddressesAndRemovesDuplicates()
    {
        client.Post<inputMessageName7, outputMessageName7>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName7>())
            .Returns(Task.FromResult(new outputMessageName7(
            [
                new WSInetAddress { ipAddress = 0x08080404 },
                new WSInetAddress { ipAddress = 0x01010101 },
                new WSInetAddress { ipAddress = 0x08080404 }
            ])));

        var result = await service.GetDnsServers();

        CollectionAssert.AreEqual(new[] { "8.8.4.4", "1.1.1.1" }, result);
        await client.Received(1).Post<inputMessageName7, outputMessageName7>(
            "ConfigurationService",
            "getDNSServers",
            "session-token",
            Arg.Any<inputMessageName7>());
    }

    [TestMethod]
    public async Task GetDnsServers_WhenControllerReturnsNoServers_ThrowsEmptyResponseException()
    {
        client.Post<inputMessageName7, outputMessageName7>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName7>())
            .Returns(Task.FromResult(new outputMessageName7 { getDNSServers1 = null! }));

        await Assert.ThrowsExactlyAsync<EmptyResponseException>(() => service.GetDnsServers());
    }

    [TestMethod]
    public async Task GetSmtpSettings_TrimsStringsAndClearsNonPositivePort()
    {
        client.Post<inputMessageName5, outputMessageName5>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName5>())
            .Returns(Task.FromResult(new outputMessageName5(new WSSMTPSettings
            {
                hostname = "  smtp.example.com  ",
                hostport = 0,
                username = "  sender  ",
                password = "  secret  "
            })));

        var result = await service.GetSmtpSettings();

        Assert.AreEqual("smtp.example.com", result.ServerAddress);
        Assert.IsNull(result.ServerPortNumber);
        Assert.AreEqual("sender", result.UserName);
        Assert.AreEqual("secret", result.Password);
    }

    [TestMethod]
    public async Task UpdateSmtpSettings_UsesDefaultPortAndForwardsSettings()
    {
        var settings = new Ihc.WebApi.Model.SmtpSettings
        {
            ServerAddress = "smtp.example.com",
            UserName = "sender",
            Password = "secret"
        };

        await service.UpdateSmtpSettings(settings);

        await client.Received(1).Post<inputMessageName4, outputMessageName4>(
            "ConfigurationService",
            "setSMTPSettings",
            "session-token",
            Arg.Is<inputMessageName4>(input =>
                input.setSMTPSettings1.hostname == "smtp.example.com" &&
                input.setSMTPSettings1.hostport == 25 &&
                input.setSMTPSettings1.username == "sender" &&
                input.setSMTPSettings1.password == "secret"));
    }
}
