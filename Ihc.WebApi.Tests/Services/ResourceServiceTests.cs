using System.Text.Json;
using Ihc.Soap.Resources;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model;
using Ihc.WebApi.Model.Resource;
using Ihc.WebApi.Services;
using NSubstitute;

namespace Ihc.WebApi.Tests.Services;

[TestClass]
public class ResourceServiceTests
{
    private IClientService client = null!;
    private IAuthCacheService authCache = null!;
    private ResourceService service = null!;

    [TestInitialize]
    public void Initialize()
    {
        client = Substitute.For<IClientService>();
        authCache = Substitute.For<IAuthCacheService>();
        service = new ResourceService(client, authCache);

        var token = Substitute.For<IAuthToken>();
        token.Token.Returns("session-token");
        authCache.GetAuthToken().Returns(token);
    }

    [TestMethod]
    [DynamicData(nameof(ResourceValueCases))]
    public async Task GetRuntimeValue_MapsSoapValue(
        WSResourceValue soapValue,
        ValueData expectedValue)
    {
        ConfigureResponse(soapValue);

        var result = await service.GetRuntimeValue(42);

        Assert.AreEqual(42, result.ResourceId);
        Assert.IsTrue(result.IsRuntimeValue);
        Assert.AreEqual("integer", result.TypeString);
        Assert.AreEqual(
            JsonSerializer.Serialize(expectedValue),
            JsonSerializer.Serialize(result.Value));
        await client.Received(1).Post<inputMessageName14, outputMessageName14>(
            "ResourceInteractionService",
            "getRuntimeValue",
            "session-token",
            Arg.Is<inputMessageName14>(input => input.getRuntimeValue1 == 42));
    }

    [TestMethod]
    public async Task GetRuntimeValue_WhenResponseIsNull_ThrowsNotFoundException()
    {
        client.Post<inputMessageName14, outputMessageName14>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName14>())
            .Returns(Task.FromResult<outputMessageName14>(null!));

        await Assert.ThrowsExactlyAsync<NotFoundException>(() => service.GetRuntimeValue(42));
    }

    [TestMethod]
    public async Task GetRuntimeValue_WhenEnvelopeIsNull_ThrowsNotFoundException()
    {
        client.Post<inputMessageName14, outputMessageName14>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName14>())
            .Returns(Task.FromResult(new outputMessageName14 { getRuntimeValue2 = null! }));

        await Assert.ThrowsExactlyAsync<NotFoundException>(() => service.GetRuntimeValue(42));
    }

    [TestMethod]
    public async Task GetRuntimeValue_WhenValueTypeIsUnknown_ReturnsNullValue()
    {
        ConfigureResponse(new UnknownResourceValue());

        var result = await service.GetRuntimeValue(42);

        Assert.IsNull(result.Value);
    }

    public static IEnumerable<object[]> ResourceValueCases =>
    [
        [
            new WSBooleanValue { value = true },
            new BooleanValueData { Value = true }
        ],
        [
            new WSIntegerValue { integer = -3, minimumValue = -10, maximumValue = 10 },
            new IntegerValueData { Value = -3, MinimumValue = -10, MaximumValue = 10 }
        ],
        [
            new WSFloatingPointValue
            {
                floatingPointValue = 1.25,
                minimumValue = 0.5,
                maximumValue = 2.5
            },
            new FloatingPointValueData { Value = 1.25, MinimumValue = 0.5, MaximumValue = 2.5 }
        ],
        [
            new WSEnumValue { definitionTypeID = 7, enumValueID = 3, enumName = "Active" },
            new EnumValueData { DefinitionTypeId = 7, EnumValueId = 3, EnumName = "Active" }
        ],
        [
            new WSTimerValue { milliseconds = 123456789 },
            new TimerValueData { Milliseconds = 123456789 }
        ],
        [
            new WSTimeValue { hours = 12, minutes = 34, seconds = 56 },
            new TimeValueData { Hours = 12, Minutes = 34, Seconds = 56 }
        ],
        [
            new WSDateValue { year = 2026, month = 8, day = 17 },
            new DateValueData { Year = 2026, Month = 8, Day = 17 }
        ],
        [
            new WSWeekdayValue { weekdayNumber = 4 },
            new WeekdayValueData { WeekdayNumber = 4 }
        ],
        [
            new WSPhoneNumberValue { number = "+1-555-0100" },
            new PhoneNumberValueData { Number = "+1-555-0100" }
        ],
        [
            new WSSceneDimmerValue { delayTime = 1, rampTime = 2, dimmerPercentage = 75 },
            new SceneDimmerValueData { DelayTime = 1, RampTime = 2, DimmerPercentage = 75 }
        ],
        [
            new WSSceneRelayValue { delayTime = 3, relayValue = true },
            new SceneRelayValueData { DelayTime = 3, RelayValue = true }
        ],
        [
            new WSSceneShutterSimpleValue { delayTime = 4, shutterPositionIsUp = true },
            new SceneShutterSimpleValueData { DelayTime = 4, ShutterPositionIsUp = true }
        ]
    ];

    private void ConfigureResponse(WSResourceValue value)
    {
        client.Post<inputMessageName14, outputMessageName14>(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<inputMessageName14>())
            .Returns(Task.FromResult(new outputMessageName14(new WSResourceValueEnvelope
            {
                resourceID = 42,
                isValueRuntime = true,
                typeString = "integer",
                value = value
            })));
    }

    private sealed class UnknownResourceValue : WSResourceValue
    {
    }
}
