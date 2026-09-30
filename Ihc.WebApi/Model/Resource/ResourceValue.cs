using System.Text.Json.Serialization;

namespace Ihc.WebApi.Model.Resource;

/// <summary>
/// Represents the value of a resource on the IHC controller.
/// </summary>
public class ResourceValue
{
    /// <summary>
    /// The resource identifier.
    /// </summary>
    /// <example>12345</example>
    public required int ResourceId { get; set; }

    /// <summary>
    /// <see langword="true"/> if the value is the current runtime value; <see langword="false"/> if it is the initial value.
    /// </summary>
    public required bool IsRuntimeValue { get; set; }

    /// <summary>
    /// The controller's name for the value type.
    /// </summary>
    public string? TypeString { get; set; }

    /// <summary>
    /// The value. The <c>type</c> property tells which kind of value it is.
    /// </summary>
    public ValueData? Value { get; set; }
}

/// <summary>
/// The base type for resource values. The <c>type</c> property identifies the kind of value.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(BooleanValueData), "boolean")]
[JsonDerivedType(typeof(IntegerValueData), "integer")]
[JsonDerivedType(typeof(FloatingPointValueData), "floatingPoint")]
[JsonDerivedType(typeof(EnumValueData), "enum")]
[JsonDerivedType(typeof(TimerValueData), "timer")]
[JsonDerivedType(typeof(TimeValueData), "time")]
[JsonDerivedType(typeof(DateValueData), "date")]
[JsonDerivedType(typeof(WeekdayValueData), "weekday")]
[JsonDerivedType(typeof(PhoneNumberValueData), "phoneNumber")]
[JsonDerivedType(typeof(SceneDimmerValueData), "sceneDimmer")]
[JsonDerivedType(typeof(SceneRelayValueData), "sceneRelay")]
[JsonDerivedType(typeof(SceneShutterSimpleValueData), "sceneShutterSimple")]
public abstract class ValueData;

/// <summary>
/// A boolean value, such as the state of an input or output.
/// </summary>
public class BooleanValueData : ValueData
{
    /// <summary>
    /// The value.
    /// </summary>
    public required bool Value { get; set; }
}

/// <summary>
/// An integer value with its allowed range.
/// </summary>
public class IntegerValueData : ValueData
{
    /// <summary>
    /// The value.
    /// </summary>
    public required int Value { get; set; }

    /// <summary>
    /// The smallest allowed value.
    /// </summary>
    public required int MinimumValue { get; set; }

    /// <summary>
    /// The largest allowed value.
    /// </summary>
    public required int MaximumValue { get; set; }
}

/// <summary>
/// A floating point value with its allowed range.
/// </summary>
public class FloatingPointValueData : ValueData
{
    /// <summary>
    /// The value.
    /// </summary>
    public required double Value { get; set; }

    /// <summary>
    /// The smallest allowed value.
    /// </summary>
    public required double MinimumValue { get; set; }

    /// <summary>
    /// The largest allowed value.
    /// </summary>
    public required double MaximumValue { get; set; }
}

/// <summary>
/// A value from an enumeration defined on the controller.
/// </summary>
public class EnumValueData : ValueData
{
    /// <summary>
    /// The identifier of the enumeration definition.
    /// </summary>
    public required int DefinitionTypeId { get; set; }

    /// <summary>
    /// The identifier of the selected enumeration value.
    /// </summary>
    public required int EnumValueId { get; set; }

    /// <summary>
    /// The name of the selected enumeration value.
    /// </summary>
    public string? EnumName { get; set; }
}

/// <summary>
/// A timer value.
/// </summary>
public class TimerValueData : ValueData
{
    /// <summary>
    /// The timer value in milliseconds.
    /// </summary>
    public required long Milliseconds { get; set; }
}

/// <summary>
/// A time of day.
/// </summary>
public class TimeValueData : ValueData
{
    /// <summary>
    /// The hours.
    /// </summary>
    public required int Hours { get; set; }

    /// <summary>
    /// The minutes.
    /// </summary>
    public required int Minutes { get; set; }

    /// <summary>
    /// The seconds.
    /// </summary>
    public required int Seconds { get; set; }
}

/// <summary>
/// A calendar date.
/// </summary>
public class DateValueData : ValueData
{
    /// <summary>
    /// The year.
    /// </summary>
    public required int Year { get; set; }

    /// <summary>
    /// The month, with January as 1.
    /// </summary>
    public required int Month { get; set; }

    /// <summary>
    /// The day of the month.
    /// </summary>
    public required int Day { get; set; }
}

/// <summary>
/// A day of the week.
/// </summary>
public class WeekdayValueData : ValueData
{
    /// <summary>
    /// The weekday number as reported by the controller.
    /// </summary>
    public required int WeekdayNumber { get; set; }
}

/// <summary>
/// A phone number.
/// </summary>
public class PhoneNumberValueData : ValueData
{
    /// <summary>
    /// The phone number.
    /// </summary>
    public string? Number { get; set; }
}

/// <summary>
/// The setting of a dimmer in a scene.
/// </summary>
public class SceneDimmerValueData : ValueData
{
    /// <summary>
    /// The delay before the scene is applied.
    /// </summary>
    public required int DelayTime { get; set; }

    /// <summary>
    /// The ramp time.
    /// </summary>
    public required int RampTime { get; set; }

    /// <summary>
    /// The dimmer level in percent.
    /// </summary>
    public required int DimmerPercentage { get; set; }
}

/// <summary>
/// The setting of a relay in a scene.
/// </summary>
public class SceneRelayValueData : ValueData
{
    /// <summary>
    /// The delay before the scene is applied.
    /// </summary>
    public required int DelayTime { get; set; }

    /// <summary>
    /// The relay state.
    /// </summary>
    public required bool RelayValue { get; set; }
}

/// <summary>
/// The setting of a shutter in a scene.
/// </summary>
public class SceneShutterSimpleValueData : ValueData
{
    /// <summary>
    /// The delay before the scene is applied.
    /// </summary>
    public required int DelayTime { get; set; }

    /// <summary>
    /// <see langword="true"/> if the shutter position is up.
    /// </summary>
    public required bool ShutterPositionIsUp { get; set; }
}
