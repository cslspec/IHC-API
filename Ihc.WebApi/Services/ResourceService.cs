using Ihc.Soap.Resources;
using Ihc.WebApi.Exceptions;
using Ihc.WebApi.Model.Resource;

namespace Ihc.WebApi.Services;

/// <summary>
/// Reads resource values from the IHC controller.
/// </summary>
public interface IResourceService
{
    /// <summary>
    /// Retrieves the current runtime value of a resource.
    /// </summary>
    /// <param name="resourceId">The resource identifier.</param>
    /// <returns>The runtime value of the resource.</returns>
    /// <exception cref="NotFoundException">The controller returns no value for the resource.</exception>
    Task<ResourceValue> GetRuntimeValue(int resourceId);
}

/// <summary>
/// Implements resource queries against the IHC ResourceInteractionService.
/// </summary>
/// <param name="client">The SOAP client used to communicate with the controller.</param>
/// <param name="authCache">Service for obtaining the current authentication token.</param>
public class ResourceService(
    IClientService client,
    IAuthCacheService authCache
    ) : IResourceService
{
    private const string ServiceName = "ResourceInteractionService";

    /// <inheritdoc />
    public async Task<ResourceValue> GetRuntimeValue(int resourceId)
    {
        var token = authCache.GetAuthToken().Token;
        var response = await client.Post<inputMessageName14, outputMessageName14>(
            ServiceName, "getRuntimeValue", token!, new inputMessageName14(resourceId));

        if (response?.getRuntimeValue2 == null)
        {
            throw new NotFoundException($"No value found for resource {resourceId}.");
        }

        var envelope = response.getRuntimeValue2;
        var result = new ResourceValue
        {
            ResourceId = envelope.resourceID,
            IsRuntimeValue = envelope.isValueRuntime,
            TypeString = envelope.typeString,
            Value = GetValueData(envelope.value)
        };

        return result;
    }

    private static ValueData? GetValueData(WSResourceValue? value) => value switch
    {
        WSBooleanValue v => new BooleanValueData { Value = v.value },
        WSIntegerValue v => new IntegerValueData
        {
            Value = v.integer,
            MinimumValue = v.minimumValue,
            MaximumValue = v.maximumValue
        },
        WSFloatingPointValue v => new FloatingPointValueData
        {
            Value = v.floatingPointValue,
            MinimumValue = v.minimumValue,
            MaximumValue = v.maximumValue
        },
        WSEnumValue v => new EnumValueData
        {
            DefinitionTypeId = v.definitionTypeID,
            EnumValueId = v.enumValueID,
            EnumName = v.enumName
        },
        WSTimerValue v => new TimerValueData { Milliseconds = v.milliseconds },
        WSTimeValue v => new TimeValueData { Hours = v.hours, Minutes = v.minutes, Seconds = v.seconds },
        WSDateValue v => new DateValueData { Year = v.year, Month = v.month, Day = v.day },
        WSWeekdayValue v => new WeekdayValueData { WeekdayNumber = v.weekdayNumber },
        WSPhoneNumberValue v => new PhoneNumberValueData { Number = v.number },
        WSSceneDimmerValue v => new SceneDimmerValueData
        {
            DelayTime = v.delayTime,
            RampTime = v.rampTime,
            DimmerPercentage = v.dimmerPercentage
        },
        WSSceneRelayValue v => new SceneRelayValueData { DelayTime = v.delayTime, RelayValue = v.relayValue },
        WSSceneShutterSimpleValue v => new SceneShutterSimpleValueData
        {
            DelayTime = v.delayTime,
            ShutterPositionIsUp = v.shutterPositionIsUp
        },
        _ => null
    };
}
